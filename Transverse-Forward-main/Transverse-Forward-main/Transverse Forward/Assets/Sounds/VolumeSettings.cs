using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] public AudioMixer musicMixer;
    [SerializeField] public Slider musicSlider;
    public VolumeNumberScript musicNumberScript;
    private float volume;

    public void setSliderVal(float value)
    {
        musicSlider.value = value;   
    }
    public void setVolume(string mixerVal)
    {
        //sets volume to the slider value
        volume = musicSlider.value;

        //converts the slider value into logarithmic scale and sets the musicMixer group volume value
        musicMixer.SetFloat(mixerVal, Mathf.Log10(volume) * 20);

        //sets the number beside the audio slider to the correct value
        musicNumberScript.setVolumeText(musicSlider.value);
        
        //saving player prefs
        if (string.Equals(mixerVal, "Master"))
        {
            PlayerPrefs.SetFloat("vMas", musicSlider.value);
            PlayerPrefs.SetString("sMas", mixerVal);
        } else if (string.Equals(mixerVal, "SFX"))
        {
            PlayerPrefs.SetFloat("vSFX", musicSlider.value);
            PlayerPrefs.SetString("sSFX", mixerVal);
        } else if (string.Equals(mixerVal, "Music"))
        {
            PlayerPrefs.SetFloat("vMus", musicSlider.value);
            PlayerPrefs.SetString("sMus", mixerVal);
        }
       
    }
}
