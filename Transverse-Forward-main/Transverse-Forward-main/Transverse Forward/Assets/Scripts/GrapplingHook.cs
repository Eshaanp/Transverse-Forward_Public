using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GrapplingHook : MonoBehaviour
{

    //need bc I need the player's position every frame
    [SerializeField] GameObject player;
    [SerializeField] private float circleRadius;

    private float playerX;
    private float playerY;

    //used to calculate where on the unit circle the hook goes
    private float mouseX;
    private float mouseY;
    private float mouseAngle;

    private float hookX;
    private float hookY;

    [SerializeField] float hookLength;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float timeBeforeHookStartup;
    [SerializeField] float grappleForce;
    [SerializeField] float maxGrappleCancelSpeed;

    [SerializeField] float swingHookDistance;
    [SerializeField] float swingSpeed;

    //debug to switch from regular hook to swing hook
    public bool isSwingHook = false;


    private void Start()
    {
        if (this.GetComponent<Rigidbody2D>() != null) this.GetComponent<Rigidbody2D>().gravityScale = 0;
    }





    //uses player position and a bunch of trigenometry to find where to put the hook on a unit circle
    void SetHookPosition()
    {
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;

        //gets the mouse position, then converts the position into Unity units (instead of pixels)
        Vector2 mouseCoordsPixel = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mouseCoordsUnits = Camera.main.ScreenToWorldPoint(mouseCoordsPixel);

        //calculate the angle the mouse is at compared to the player
        mouseX = mouseCoordsUnits.x - playerX;
        mouseY = mouseCoordsUnits.y - playerY;
        mouseAngle = Mathf.Atan2(mouseY, mouseX);

        //set the hook's position to the corresponding unit circle coords for the respective mouse angle
        hookX = playerX + Mathf.Cos(mouseAngle) * circleRadius;
        hookY = playerY + Mathf.Sin(mouseAngle) * circleRadius;
        transform.position = new Vector2(hookX, hookY);
        transform.rotation = Quaternion.FromToRotation(Vector2.right, (transform.position - player.transform.position).normalized);
    }


    void ShootHook()
    {
        RaycastHit2D chain = Physics2D.Raycast(new Vector2(hookX, hookY), new Vector2(Mathf.Cos(mouseAngle), Mathf.Sin(mouseAngle)), hookLength);
        if (chain.collider != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, new Vector2(hookX, hookY));
            lineRenderer.SetPosition(1, chain.point);

            if (!isSwingHook) StartCoroutine(GetDraggedCoroutine(chain.point));
            else StartCoroutine(SwingHookCoroutine(chain.point));
        }
    }

 
    private IEnumerator GetDraggedCoroutine(Vector2 chainPoint)
    {
        yield return new WaitForSeconds(timeBeforeHookStartup);
        while (Input.GetKey(KeyCode.Mouse0))
        {
            //updating the grapple line
            lineRenderer.SetPosition(0, new Vector2(hookX, hookY));

            //updating velocity based on how far away you are
            Vector3 direction = (Vector3)chainPoint - player.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * grappleForce;
            yield return new WaitForEndOfFrame();
        }

        //getting rid of the grapple line
        lineRenderer.positionCount = 0;

        //limiting the amount of momentum you can get after cancelling a grapple
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x) > maxGrappleCancelSpeed)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * maxGrappleCancelSpeed, player.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y) > maxGrappleCancelSpeed)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, vertical * maxGrappleCancelSpeed);
        }
    }


    /*
     * TODO:
     * Fix the player spazzing out when connected
     * Make the player gradually get to the swing position instead of teleporting there
     * Stop the player from targeting anything under -45 or above 45 degrees w/ the swing grapple
     * Add some 'physics' to the swing so you get momentum from pressing back and forth (instead of teleporting to different positions)
     * Add some 'physics' to the swing so it goes back to 0 degrees if you don't press anything
     * Create a way to cancel the swing (instead of the debug method of pressing '1')
     * Tinker with swingHookDistance and swingSpeed values to find what feels good
     */
    private IEnumerator SwingHookCoroutine(Vector2 chainPoint)
    {
        yield return new WaitForSeconds(timeBeforeHookStartup);

        float tempGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;

        //calculate the angle player is at compared to chain point (except this time sin and cos are swapped for some reason) (too lazy to fix)
        float chainPointAngle = Mathf.Atan2(chainPoint.x - player.transform.position.x, chainPoint.y - player.transform.position.y);
        //Debug.Log("chainPointAngle = " + chainPointAngle * Mathf.Rad2Deg + "; cos(theta) = " + Mathf.Cos(chainPointAngle) + "; sin(theta) = " + Mathf.Sin(chainPointAngle));

        while (isSwingHook)
        {
            chainPointAngle += swingSpeed * Input.GetAxisRaw("Horizontal") * -1;

            player.transform.position = new Vector2(chainPoint.x - swingHookDistance * Mathf.Sin(chainPointAngle), chainPoint.y - swingHookDistance * Mathf.Cos(chainPointAngle));

            lineRenderer.SetPosition(0, player.transform.position);

            yield return new WaitForSeconds(0.05f);
        }

        player.GetComponent<Rigidbody2D>().gravityScale = tempGravityScale;
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!isSwingHook) isSwingHook = true;
            else isSwingHook = false;
        }

        if (Time.timeScale > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootHook();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            Debug.Log("X: " + worldPosition.x + "; Y: " + worldPosition.y);
        }
    }


    void FixedUpdate()
    {
        SetHookPosition();
        bool facingRight = player.GetComponent<PlayerMovement>().facingRight;
        if (facingRight && transform.position.x < player.transform.position.x || !facingRight && transform.position.x > player.transform.position.x) {
            player.GetComponent<PlayerMovement>().Turn();
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }
}
