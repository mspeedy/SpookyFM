using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    string gameLevel;
	// Use this for initialization
	void Start () {
        gameLevel = "Spook";
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(gameLevel);
        }

    }
}
