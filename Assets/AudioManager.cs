using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    private void Start() {
        source.Play();
    }
}

