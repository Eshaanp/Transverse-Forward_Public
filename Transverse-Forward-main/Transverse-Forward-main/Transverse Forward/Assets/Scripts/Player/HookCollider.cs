using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollider : MonoBehaviour
{
    public bool taut;
    public DistanceJoint2D joint;
    public bool returning = false;
    GameObject gAnchor;
    GameObject shootPoint;
    public GameObject spark;
    float hSpeed;

    private void Start()
    {
        gAnchor = GameObject.Find("GrappleAnchor");
        shootPoint = GameObject.Find("Launch Point");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.name == "Ground")
        {
            gAnchor.GetComponent<JGrappleHook>().hooked = true;
            GameObject.Find("Player").GetComponent<PlayerMovement>().currentHook = gameObject;
            GameObject.Find("Player").GetComponent<PlayerMovement>().hooked = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            joint.connectedBody = gAnchor.transform.parent.GetComponent<Rigidbody2D>();
            joint.enabled = false;
        }
        if(collision.collider.gameObject.name == "Hazardous")
        {
            gAnchor.GetComponent<JGrappleHook>().Reel();
            Instantiate(spark, transform.position, Quaternion.identity);
        }
    }

    public void FixedUpdate()
    {
        if (returning) {
            GetComponent<Rigidbody2D>().velocity = (shootPoint.transform.position - transform.position).normalized * hSpeed;
        } else {
            joint.enabled = taut;
        }
    }

    private void Update()
    {
        if (returning && (shootPoint.transform.position - transform.position).magnitude < .1f)
        {
            gAnchor.GetComponent<JGrappleHook>().doneReeling = true;
            gAnchor.GetComponent<JGrappleHook>().hooked = false;
        }
    }

    public void Returning(float hookSpeed)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        returning = true;
        hSpeed = hookSpeed;
        joint.enabled = false;
    }
}
