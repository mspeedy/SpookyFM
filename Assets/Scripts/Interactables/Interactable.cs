using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	
	public enum InteractableTypes { MusicButton, CoffeeMachine, WifiRouter }

	private BoxCollider2D boxCollide;
	public InteractableTypes type;
	private GameObject Camera;
	private AudioSource triggeredSound;
    Behaviour halo ;

    // Use this for initialization
    void Start () {
        boxCollide = GetComponent<BoxCollider2D> ();
        triggeredSound = GetComponent<AudioSource> ();
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
	}

	void OnTriggerStay2D (Collider2D other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement> ();
        if (player != null)
        {
            halo.enabled = true;
            CheckInteraction(player);
        }

	}

    void OnTriggerExit2D(Collider2D other)
    {
        halo.enabled = false;
    }
		
	public void CheckInteraction (PlayerMovement player) {
		AudioManager audioman = Camera.GetComponent<AudioManager>();
		if (Input.GetButtonDown ("Space")) {
			if (type == InteractableTypes.CoffeeMachine) {
				SpookyManager.restoreCaffeine ();
				triggeredSound.Play ();
			}
			else if (type == InteractableTypes.WifiRouter) {
				if (SpookyManager.iscomcastshit) {
					Debug.Log ("Wifi being fixed.");
					SpookyManager.fixWifi ();
					triggeredSound.Play ();
				}
			}

		} else if (Input.GetButtonDown ("Music1") && type == InteractableTypes.MusicButton) {
			Debug.Log ("You touched the red button");
			if (SpookyManager.changeState (MusicStateManager.GameStates.Red)) {
				audioman.playMusic (MusicStateManager.GameStates.Red);
			}
		} else if (Input.GetButtonDown ("Music2") && type == InteractableTypes.MusicButton) {
			if (SpookyManager.changeState (MusicStateManager.GameStates.Green)) {
				audioman.playMusic (MusicStateManager.GameStates.Green);
			}
		} else if (Input.GetButtonDown ("Music3") && type == InteractableTypes.MusicButton) {
			if (SpookyManager.changeState (MusicStateManager.GameStates.Blue)) {
				audioman.playMusic (MusicStateManager.GameStates.Blue);
			}
		}
	}

}

