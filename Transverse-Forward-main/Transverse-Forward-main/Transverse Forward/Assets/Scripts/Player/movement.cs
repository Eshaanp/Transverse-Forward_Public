using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{


    public float speed = 3f; 
    public GameObject diamond; // poop
    Rigidbody2D rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = diamond.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxisRaw("Horizontal");



    }

    


    private void FixedUpdate()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        rBody.velocity = new Vector2(hori * speed, 0);


    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }





}
