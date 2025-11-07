using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject player;
    
    public bool isDead;

    //if player collides with hazard body, set main cam and player to begining
    //this wont be in final verison, will need spawnpoint for each section


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        //if player collides with hazard, call toCheckpoint()
        if (collision.gameObject.name == "Player")
        {
            player.transform.position = new Vector3(67, 41, 0);
            isDead = true;

        }      

   
    }

   // private void spawn(Boolean dead);









}
