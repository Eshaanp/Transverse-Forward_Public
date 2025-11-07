using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeNumberScript : MonoBehaviour
{
    public TextMeshProUGUI volText;
    [SerializeField] public float volumeNumber;

    public void setVolumeText(float volumeNum)
    {
        //gets the slider value that was passed in
        volumeNumber = volumeNum;

        //formats text
        volText.text = "";
        volText.fontSize = 17;

        //displays the text 
        volText.text += (int)(volumeNumber * 100);

    }

}
