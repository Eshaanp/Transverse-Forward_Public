using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChooser : MonoBehaviour
{
    public AudioSource caveMusic;
    public AudioSource forestMusic;

    Coroutine caveSwitch;
    Coroutine forestSwitch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if(collision.gameObject.name == "Cave Audio")
        {
            caveSwitch = StartCoroutine(FtoC());
            if(forestSwitch != null)
            {
                StopCoroutine(forestSwitch);
            }
        }
        if (collision.gameObject.name == "Outdoor Audio")
        {
            forestSwitch = StartCoroutine(CtoF());
            if (caveSwitch != null)
            {
                StopCoroutine(caveSwitch);
            }
        }
    }

    IEnumerator FtoC()
    {
        for(int i = 0; i < 100; i++)
        {
            if(caveMusic.volume == 0)
            {
                caveMusic.Play();
            }
            if(caveMusic.volume + .01f <= 1)
            {
                caveMusic.volume += .01f;
            }
            if (forestMusic.volume - .01f >= 0)
            {
                forestMusic.volume -= .01f;
            }
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator CtoF()
    {
        for (int i = 0; i < 100; i++)
        {
            if (forestMusic.volume == 0)
            {
                forestMusic.Play();
            }
            if (forestMusic.volume + .01f <= 1)
            {
                forestMusic.volume += .01f;
            }
            if (caveMusic.volume - .01f >= 0)
            {
                caveMusic.volume -= .01f;
            }
            yield return new WaitForSeconds(.01f);
        }
    }
}
