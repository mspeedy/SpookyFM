using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashToTitle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke ("swapScene", 5);
	}

    void swapScene() {
        SceneManager.LoadScene("TitleScreen");
    }
}
