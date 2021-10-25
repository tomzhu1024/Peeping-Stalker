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

	// Start is called before the first frame update
	void Start()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		// see if the user has pressed X, if it is , make the character to crounch
		ispressed();
		// read input from the user
		InputHorizontal = Input.GetAxis("Vertical");
		InputVertical = Input.GetAxis("Horizontal");
		// for Animation
		triggeranimate(InputHorizontal,InputVertical);
		// move character
		move(InputHorizontal,InputVertical);
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
	public void ispressed()
	{
		//Debug.Log(Input.GetKeyDown(KeyCode.X));
		if (Input.GetKeyDown(KeyCode.X))
		{
			_animator.SetBool("IsCrounched", true);
			if (Input.GetKeyUp(KeyCode.X))
			{
				_animator.SetBool("IsCrounched", false);
			}
		}
		
	}
}



