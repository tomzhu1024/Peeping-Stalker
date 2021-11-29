using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	[SerializeField] private float speed;
	//private Animator _animator;
	private CharacterController _controller;
	private Vector3 movedirectionZ;
	private Vector3 movedirectionX;
	private float InputHorizontal;
	private float InputVertical;

	// Start is called before the first frame update
	void Start()
	{
		//_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		// read input from the user
		InputHorizontal = Input.GetAxis("Vertical");
		InputVertical = Input.GetAxis("Horizontal");
		// for Animation
		//triggeranimate(InputHorizontal,InputVertical);
		// move character
		move(InputHorizontal,InputVertical);
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
}

