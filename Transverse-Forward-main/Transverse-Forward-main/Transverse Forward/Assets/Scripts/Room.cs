using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{

    public GameObject camera;
    //public GameObject player;
    public Boolean hasMovingObject;
    public Boolean isInside;


    void Start()
    {
        
    }
    void Update()
    {
           //camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            camera.SetActive(true);
            isInside = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            camera.SetActive(false);
            isInside = false;
        }
    }

}
