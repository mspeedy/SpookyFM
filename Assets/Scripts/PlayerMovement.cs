using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed;
	public bool facingRight;
    private Rigidbody2D rigid;
    private BoxCollider2D toucher;
    private int caffeinestate;
    private Animator anim;
    private bool isMoving;
	public AudioSource walkingSound;
	public AudioSource heartbeat;


	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        toucher = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator> ();
		speed = .1f;
		facingRight = false;
	}

	// Update is called once per frame
	void Update ()
	{
        caffeinestate = SpookyManager.getCaffeineState();
        float tempspeed = speed * (.6f + (caffeinestate * .15f));
        anim.SetInteger ("CaffieneState", caffeinestate);
		if (caffeinestate == 1) {
			if (!heartbeat.isPlaying) {
				heartbeat.Play ();
			}
		}
		else {
			heartbeat.Stop ();
		}

		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
            anim.SetBool ("InMotion", true);
			transform.position = new Vector2 (transform.position.x - tempspeed, transform.position.y);
			if (facingRight) {
				Flip (); 
			}
			if (!walkingSound.isPlaying) {
				walkingSound.Play ();
			}
		} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
            anim.SetBool ("InMotion", true);
			transform.position = new Vector2 (transform.position.x + tempspeed, transform.position.y);
			if (!facingRight) {
				Flip ();
			}
			if (!walkingSound.isPlaying) {
				walkingSound.Play ();
			}
        } else {
            anim.SetBool ("InMotion", false);
			walkingSound.Stop ();
        }

	}

	void Flip()
	{
		if (facingRight) {
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		} else {
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}

}
