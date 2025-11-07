using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Expires());
    }

    IEnumerator Expires()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
