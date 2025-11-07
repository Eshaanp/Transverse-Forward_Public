using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    
    public GameObject respawnPoint;
    public GameObject player;
    public bool isDead;


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player.GetComponent<DeathAnim>().Die(respawnPoint);
            isDead = true;
        }
    }
}
