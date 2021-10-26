using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
	[SerializeField] private float speed;
	private Animator _animator;
	private CharacterController _controller;
	private Vector3 movedirectionZ;
	private Vector3 movedirectionX;
	private float InputHorizontal;
	private float InputVertical;
	private AudioSource heart; //heartbeat  sound
	// Start is called before the first frame update
	void Start()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();
		heart = GetComponent<AudioSource>();

		StartCoroutine(soundCoroutine());
	}

	// Update is called once per frame
	void Update()
	{
		// read input from the user
		InputHorizontal = Input.GetAxis("Vertical");
		InputVertical = Input.GetAxis("Horizontal");
		// for Animation
		triggeranimate(InputHorizontal,InputVertical);
		// move character
		move(InputHorizontal,InputVertical);

		//get distance from manager
	}
	private void triggeranimate(float Z_direction,float X_direction)
	{
		if (Z_direction > 0)
		{
			_animator.SetFloat("MoveSpeed", Z_direction);
		}
		else
		{
			_animator.SetFloat("MoveSpeed", X_direction);
		}
	}
	private void move(float Z_direction,float X_direction)
	{
		// Z axis 
		movedirectionZ = new Vector3(0, 0, Z_direction);
		movedirectionZ *= speed;
		_controller.Move(movedirectionZ * Time.deltaTime);
		
		// X axis
		movedirectionX = new Vector3(X_direction, 0, 0);
		movedirectionX *= speed;
		_controller.Move(movedirectionX * Time.deltaTime);
	}


	private void playSound(){

	}

	IEnumerator soundCoroutine(){
		float prevDist = 15;
		while(true){
			Debug.Log(manager.dist);
			
			heart.Play();

			float interval = manager.dist < prevDist ? 0.5f : 2f;
			prevDist = manager.dist;
			yield return new WaitForSeconds(interval);

		}
	}
}



