﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    private readonly int Scene_Level1 = 1;

    public static GameManager instance {
        get {
            if (_instance == null ) {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }


    private void Awake() {
        //Check if instance already exists
        if (_instance == null) {
            //if not, set instance to this
            _instance = this;
        } else if (_instance != this) { //If instance already exists and it's not this:
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }


    public void NextLevel() {
        ChangeScence(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public Image blackScreen;
    public float fadeTime = 2f;

    public IEnumerator ValdoSpotted() {
        // fondu au noir 
        if (blackScreen == null) {
            blackScreen = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<Image>();
        }
        for (float i = 0; i <= fadeTime; i += Time.deltaTime) {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }

        yield return new WaitForSeconds(fadeTime);
        //respawn de la scene  ou Warp de valdo ?
        if (blackScreen.color.a > 0.9f) {
            ChangeScence(Scene_Level1);
        }

    }

 

    public void ChangeScence(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }


}
