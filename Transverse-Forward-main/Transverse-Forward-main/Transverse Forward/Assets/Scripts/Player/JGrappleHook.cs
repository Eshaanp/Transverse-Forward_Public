using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JGrappleHook : MonoBehaviour
{
    public GameObject hook;
    public GameObject launchPoint;
    public float hookSpeed;
    public bool hooked;
    public Sprite unShot;
    public Sprite shot;
    public LineRenderer lrender;
    public bool hasHook = true;
    public bool doneReeling = false;
    float maxShootDistance;

    public enum States { None, Shooting, Hooked, Reeling };
    public States state;
    public GameObject currentHook;
    // Start is called before the first frame update
    void Start()
    {
        state = States.None;
        maxShootDistance = transform.parent.gameObject.GetComponent<PlayerMovement>().maxShootDistance;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case States.None:
                if(transform.parent.GetComponent<SpriteRenderer>().enabled == true)
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    transform.right = (Vector2)worldPosition - (Vector2)transform.position;
                    if (Input.GetMouseButton(0) && hasHook)
                    {
                        currentHook = Instantiate(hook, launchPoint.transform);
                        currentHook.transform.parent = null;
                        currentHook.GetComponent<Rigidbody2D>().velocity = ((Vector2)worldPosition - (Vector2)transform.position).normalized * hookSpeed;
                        state = States.Shooting;
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shot;
                        lrender.enabled = true;
                    }
                }
                else
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                }
                break;
            case States.Shooting:
                transform.right = (Vector2)currentHook.transform.position - (Vector2)transform.position;
                if(hooked)
                {
                    state = States.Hooked;
                }
                DrawLine();
                if(Input.GetMouseButtonUp(0) || (currentHook.transform.position - transform.position).magnitude > maxShootDistance)
                {
                    state = States.Reeling;
                    currentHook.GetComponent<HookCollider>().Returning(hookSpeed);
                    GameObject.Find("Player").GetComponent<PlayerMovement>().hookState = PlayerMovement.HookStates.None;
                }
                break;
            case States.Hooked:
                DrawLine();
                if (Input.GetMouseButtonUp(0))
                {
                    state = States.Reeling;
                    currentHook.GetComponent<HookCollider>().Returning(hookSpeed);
                    GameObject.Find("Player").GetComponent<PlayerMovement>().hooked = false;
                }
                transform.right = (Vector2)currentHook.transform.position - (Vector2)transform.position;
                break;
            case States.Reeling:
                transform.right = (Vector2)currentHook.transform.position - (Vector2)transform.position;
                DrawLine();
                if (doneReeling)
                {
                    GameObject.Destroy(currentHook);
                    hasHook = true;
                    doneReeling = false;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = unShot;
                    lrender.enabled = false;
                    state = States.None;
                }
                break;
        }
        bool facingRight = GetComponentInParent<PlayerMovement>().facingRight;
        if (facingRight && transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270 || !facingRight && !(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) && transform.eulerAngles.y == 0) {
            GetComponentInParent<PlayerMovement>().Turn();
            GetComponentInChildren<SpriteRenderer>().flipY = !GetComponentInChildren<SpriteRenderer>().flipY;
        }
    }

    public void Reel()
    {
        state = States.Reeling;
        currentHook.GetComponent<HookCollider>().Returning(hookSpeed);
        GameObject.Find("Player").GetComponent<PlayerMovement>().hooked = false;
    }

    void DrawLine()
    {
        lrender.SetPosition(0, currentHook.transform.position);
        lrender.SetPosition(1, launchPoint.transform.position);
    }
}
