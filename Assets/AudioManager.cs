using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioM;
    private void Awake()
    {
        audioM = GetComponent<AudioSource>();
        if (GameManager.sound)
        {
            audioM.mute = false;
        }
        else
        {
            audioM.mute = true;
        }

        audioM.Play();
    }

    void Update()
    {
        if (GameManager.sound)
        {
            audioM.mute = false;
            if (!audioM.isPlaying)
            {
                audioM.UnPause();
            }
        }
        else
        {
            audioM.mute = true;
            if (audioM.isPlaying)
            {
                audioM.Pause();
            }          
        }
    }

    public void CallMuteOnGm()
    {
        GameManager.instance.ToggleMute();
    }
}
