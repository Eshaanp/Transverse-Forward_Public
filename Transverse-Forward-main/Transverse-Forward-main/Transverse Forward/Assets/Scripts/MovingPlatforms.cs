using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class MovingPlatforms : MonoBehaviour
{


    public GameObject room;
    public GameObject point1;
    public GameObject point2;
    public float speed;
    
    public Boolean isWorking;
    public enum dir
    {
        up,
        down,
        left,
        right,
        northEast,
        southEast,
        northWest,
        southWest,
    }
    public dir dropDown = new dir();


    //public GameObject floor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (room.GetComponent<ProjectileZone>().isInside == true)
        {
            if (dropDown == dir.up)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else if (dropDown == dir.down)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else if (dropDown == dir.right)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else if (dropDown == dir.left)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else if (dropDown == dir.northEast)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else if (dropDown == dir.southEast)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else if (dropDown == dir.northWest)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else if (dropDown == dir.southWest)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
        }
    }

    
    /*
    private void North()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);

       
    }

    private void South()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);

      /*  if (transform.position.y < end)
        {
            isWorking = true;
            transform.position = new Vector3(transform.position.x, start, transform.position.z);
        }
    }
    private void East()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);

       /* if (transform.position.x > end)
        {
            isWorking = true;
            transform.position = new Vector3(start, transform.position.y, transform.position.z);
        }
    }
    private void West()
    {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);

        /*if (transform.position.x < 3)
        {
            isWorking = true;
            transform.position = new Vector3(start, transform.position.y, transform.position.z);
        }
    } */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "projectileEndpoint")
        {
            transform.position = new Vector3(point1.transform.position.x, point1.transform.position.y, point1.transform.position.z);


        }
    }

}
