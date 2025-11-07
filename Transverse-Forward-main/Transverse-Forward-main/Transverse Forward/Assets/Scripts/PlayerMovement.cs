using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Components
    private Rigidbody2D rbody;
    private BoxCollider2D playerCollider;
    private AudioSource jumpSFX;

    //Variables
    public float speed;
    public float acceleration;
    public float deceleration;
    public float jumpForce;
    public float jumpCooldown;
    public float normalGravity;
    public float slamGravity;
    public bool hooked;
    public float swingSpeed;
    public float minHookDistance;
    public float reelingSpeed;
    public float maxShootDistance;
    public GameObject circle;

    //Private Variables
    private float currentAcceleration;
    private bool canJump;
    private LayerMask groundLayer;
    private bool longJump;
    private Animator animator;
    [HideInInspector] public bool facingRight = true;
    public enum HookStates { None, Taut, Slack, Grounded};
    public HookStates hookState;
    public GameObject currentHook;
    private HookCollider hCollider;
    private bool grounded = false;

    void Start()
    {
        circle.GetComponent<Animator>().Play("CircleOpen");
        hookState = HookStates.None;
        rbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        canJump = true;
        hooked = false;
    }

    private void Update()
    {
        CheckGrounded();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            longJump = false;
        }
        animator.SetFloat("Speed", Mathf.Abs(rbody.velocity.x));
        animator.SetFloat("VV", rbody.velocity.y);
        animator.SetBool("Grounded", isGrounded());
        animator.SetBool("RunningBW", facingRight && rbody.velocity.x < 0 || !facingRight && rbody.velocity.x > 0);
    }

    void FixedUpdate()
    {
        switch(hookState)
        {
            case HookStates.None:
                NormalMovement();
                if (hooked && rbody.velocity.y <= 0 && currentHook != null)
                {
                    hCollider = currentHook.GetComponent<HookCollider>();
                    hookState = HookStates.Slack;
                }
                break;
            case HookStates.Slack:
                if(isGrounded())
                {
                    hookState = HookStates.Grounded;
                }
                else if(currentHook != null && currentHook.transform.position.y > transform.position.y)
                {
                    hookState = HookStates.Taut;
                    hCollider.taut = true;
                }
                else if (!hooked)
                {
                    hookState = HookStates.None;
                }
                break;
            case HookStates.Taut:
                if(isGrounded() && Input.GetAxisRaw("Vertical") <= 0)
                {
                    hookState = HookStates.Grounded;
                    hCollider.taut = false;
                }
                else if(currentHook != null && currentHook.transform.position.y < transform.position.y)
                {
                    hookState = HookStates.Slack;
                    hCollider.taut = false;
                }
                else if(!hooked)
                {
                    rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y + 6f);
                    hookState = HookStates.None;
                }
                else
                {
                    HookMovement();
                    Climbing();
                }
                break;
            case HookStates.Grounded:
                Climbing();
                NormalMovement();
                if (!isGrounded())
                {
                    hookState = HookStates.Slack;
                }
                else if (!hooked)
                {
                    hookState = HookStates.None;
                }
                break;
        }
    }

    void Climbing()
    {
        if(hooked && currentHook != null)
        {
            float reeling = Input.GetAxisRaw("Vertical") * reelingSpeed;
            if(reeling > 0 && hookState == HookStates.Grounded && currentHook.transform.position.y > transform.position.y)
            {
                hookState = HookStates.Taut;
                hCollider.taut = true;
            }
            float distance = currentHook.GetComponent<DistanceJoint2D>().distance - reeling * Time.fixedDeltaTime;
            if (distance > minHookDistance && distance < maxShootDistance)
            {
                currentHook.GetComponent<DistanceJoint2D>().distance -= reeling * Time.fixedDeltaTime;
            }
        }
        //Debug.Log("!");
    }

    private void HookMovement()
    {
        Vector2 goalSpeed = new Vector2(Input.GetAxisRaw("Horizontal") * swingSpeed * Time.fixedDeltaTime, 0);
        rbody.velocity += goalSpeed;
    }

    private void NormalMovement()
    {
        //if(isGrounded())
        //{
            Vector2 goalSpeed = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rbody.velocity.y);
            if (goalSpeed.magnitude < rbody.velocity.magnitude)
            {
                currentAcceleration = deceleration;
            }
            else
            {
                currentAcceleration = acceleration;
            }
            if (isGrounded() || !(Mathf.Sign(goalSpeed.x * rbody.velocity.x) > 0 && Mathf.Abs(rbody.velocity.x) > Mathf.Abs(goalSpeed.x)))
                rbody.velocity = Vector2.Lerp(rbody.velocity, goalSpeed, currentAcceleration * Time.fixedDeltaTime);
            if (rbody.velocity.y < 1f || !longJump)
            {
                rbody.gravityScale = slamGravity;
            }
            else
            {
                rbody.gravityScale = normalGravity;
            }
            // If Turning
            /*if (((goalSpeed.x < 0 && facingRight) || (goalSpeed.x > 0 && !facingRight))) {
                Turn();
            }*/
        /*else
        {
            float horiz = Input.GetAxisRaw("Horizontal") * speed;
            if(Mathf.Sign(horiz) != Mathf.Sign(rbody.velocity.x))
            {
                horiz *= 2;
            }
            rbody.velocity = new Vector2(rbody.velocity.x + horiz * Time.fixedDeltaTime, rbody.velocity.y);
        }*/
    }

    private void Jump()
    {
        if(canJump && isGrounded())
        {
            rbody.velocity += new Vector2(0, jumpForce);
            canJump = false;
            StartCoroutine(jumpCooldownCoroutine());
            if(Input.GetKeyDown(KeyCode.Space))
            {
                longJump = true;
            }

            jumpSFX = GetComponent<AudioSource>();
            jumpSFX.Play();

        }
        
    }

    IEnumerator jumpCooldownCoroutine()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private bool isGrounded()
    {
        return grounded;
    }

    private void CheckGrounded()
    {
        grounded = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .02f, groundLayer);
    }

    public void Turn() {
        facingRight = !facingRight;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }
}
