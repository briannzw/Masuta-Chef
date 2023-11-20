using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClikButton : MonoBehaviour
{
    public AudioSource audioClikButton;

    public void playThisSoundEffect()
    {
        audioClikButton.Play();
    }
}
