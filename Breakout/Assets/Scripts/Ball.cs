﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public AudioClip ball;
	public bool bInPlay = false; // maybe levelManager should control this too?
	public int cBallsRemaining;

	private float currentVelocityX;
	private float currentVelocityY;
	private float maxVelocityX = 12f;
	private float maxVelocityY = 19f;
	private Paddle paddle;
	private Vector3 PaddleToBallVector;
	private Vector2 preClampVelocity;

	void Start () {
		// TODO bug testing and architechture descision
		// do we want LevelManager to be responsible for this variable? probably.... 
		// I suspect here it resets to 3 balls each level, 
		// while if it were in the level-manager it should be persistent(?)
		cBallsRemaining = 3; 
		paddle = GameObject.FindObjectOfType<Paddle>();
		PaddleToBallVector = this.transform.position - paddle.transform.position;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			bInPlay = true;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (2f, 10f);
		}
		else if (!bInPlay) this.transform.position = paddle.transform.position + PaddleToBallVector;
	}

	void CueAudio () {
		// GetComponent<AudioSource>().Play();
		AudioSource.PlayClipAtPoint (ball, transform.position); // optional 3rd float value for volume
	}

		// this is here (mostly) for two purposes. 1: clamp velocity. 2: prevent bounce-looping with some random bounce jarring
	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 tweak = new Vector2 (Random.Range(-0.15f, 0.15f), Random.Range(-0.25f, 0.25f));
		if (bInPlay) {
			CueAudio(); // the "other" thing that happens here
			preClampVelocity = (GetComponent<Rigidbody2D>().velocity += tweak);
			currentVelocityX = Mathf.Clamp (preClampVelocity.x, -maxVelocityX, maxVelocityX);
			currentVelocityY = Mathf.Clamp (preClampVelocity.y, -maxVelocityY, maxVelocityY);
			GetComponent<Rigidbody2D>().velocity = new Vector2 (currentVelocityX, currentVelocityY);
		}
	}
}
