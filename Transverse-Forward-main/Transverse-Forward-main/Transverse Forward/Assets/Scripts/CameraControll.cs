using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public GameObject player;
    public GameObject hazard;
    public float speed; 

    //thought for later- would be cool if camera dragged itself to beginngig of section
    

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        
        
        
        /*
        if(hazard.GetComponent<Hazard>().isDead == false)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        }
        
        else {
            transform.position = new Vector3(0, 0, -10);

            //algorithm to move (not tp cam to position) 

            hazard.GetComponent<Hazard>().isDead = false;

        } */


    }









}
