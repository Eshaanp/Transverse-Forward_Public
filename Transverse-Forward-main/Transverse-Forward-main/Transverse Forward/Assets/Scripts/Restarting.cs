using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarting : MonoBehaviour
{
    public GameObject circle;
    bool ending = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !ending)
        {
            StartCoroutine(EndCo());
            ending = true;
        }
    }

    IEnumerator EndCo()
    {
        circle.GetComponent<Animator>().Play("CircleClose");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
