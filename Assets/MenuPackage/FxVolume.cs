using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FxVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("FxVolume", volume);
        PlayerPrefs.SetFloat("FxVolume",volume);
    }
}
