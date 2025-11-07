using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{


    private Respawn respawnChange;
    private Respawn respawnProj;
    public bool isWork;

    private void Awake()
    {
        respawnChange = GameObject.FindGameObjectWithTag("hazard").GetComponent<Respawn>();
        respawnProj = GameObject.FindGameObjectWithTag("Projectile").GetComponent<Respawn>();

    }

    void Start()
    {
        isWork = false;
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isWork = true;
            respawnChange.respawnPoint = this.gameObject;
            respawnProj.respawnPoint = this.gameObject;
        }
    }
}
