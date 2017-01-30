using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static Interactable.InteractableTypes typeUsed;
	public string path;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playMusic(MusicStateManager.GameStates state) {
		stopMusic ();
		int rand = Random.Range (0, 2);
		var aSources = gameObject.GetComponents<AudioSource> ();
		if (state == MusicStateManager.GameStates.Blue) {
			aSources [1 + rand].Play ();
		} else if (state == MusicStateManager.GameStates.Red) {
			aSources [3 + rand].Play ();
		} else if (state == MusicStateManager.GameStates.Green) {
			aSources [5 + rand].Play ();
		}
	}

	public void stopMusic() {
		var aSources = gameObject.GetComponents<AudioSource> ();
		for(int i = 1; i < aSources.Length; i++) {
			aSources [i].Stop ();
		}
	}

}

