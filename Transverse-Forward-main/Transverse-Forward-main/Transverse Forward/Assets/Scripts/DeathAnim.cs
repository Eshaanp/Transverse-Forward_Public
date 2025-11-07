using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathAnim : MonoBehaviour
{
    public GameObject circle;
    public GameObject deathSound;
    public TextMeshProUGUI text;
    Rigidbody2D rbody;
    JGrappleHook pMove;
    private bool dying = false;
    private int deaths = 0;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        pMove = transform.GetChild(0).GetComponent<JGrappleHook>();
    }

    // Update is called once per frame
    public void Die(GameObject respawn)
    {
        if(!dying)
        {
            dying = true;
            Instantiate(deathSound, transform);
            rbody.velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<ParticleSystem>().Play();
            if (pMove.currentHook != null)
            {
                GameObject hook = pMove.currentHook;
                hook.GetComponent<HookCollider>().Returning(10);
                pMove.state = JGrappleHook.States.Reeling;
            }
            circle.GetComponent<Animator>().Play("CircleClose");
            StartCoroutine(DieCo(respawn));
        }
    }

    IEnumerator DieCo(GameObject respawn)
    {
        yield return new WaitForSeconds(1f);
        circle.GetComponent<Animator>().Play("CircleOpen");
        if (pMove.currentHook != null)
        {
            pMove.doneReeling = true;
            pMove.hooked = false;
        }
        transform.position = respawn.transform.position;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
        dying = false;
        deaths++;
        text.text = deaths.ToString();
    }
}
