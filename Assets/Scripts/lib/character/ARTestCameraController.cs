using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARTestCameraController : MonoBehaviour
{
	public bool enabled = false;

	[Header("View")] 
	public Camera viewCamera;
	[Range(1, 1000)] 
	public float mouseSensitivity = 500f;
	public bool invertMouse = false;

	[Header("Movement")] 
	[Range(0,.1f)]
	public float moveSpeed = .003f;

	private float _xRotation = 0f;
		
	public void Start()
	{
		if (!Application.isEditor || !enabled )
			return;

		if (!viewCamera)
			viewCamera = Camera.main;
	}

	void Update()
	{
		if (!Application.isEditor )
			return;

		if (Input.GetMouseButtonDown(1))
		{
			enabled = true;
			Cursor.lockState = CursorLockMode.Locked;
		}
		else if (Input.GetMouseButtonUp(1))
		{
			enabled = false;
			Cursor.lockState = CursorLockMode.None;
		}

		if (enabled)
		{
			UpdateLook();
			UpdatePosition();
		}
	}

	void UpdateLook()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		// Up/down
		_xRotation += invertMouse ? mouseY : -mouseY;
		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
		if ( viewCamera )
			viewCamera.transform.localRotation = Quaternion.Euler( _xRotation , 0 , 0 );
		
		// Left/right
		transform.Rotate( Vector3.up * mouseX );
	}

	void UpdatePosition()
	{
		float xDelta = 0;
		float yDelta = 0;
		float zDelta = 0;

		float multiplier = 1;

		if (Input.GetKey(KeyCode.LeftShift))
			multiplier = 2f;
		
		// Right
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			xDelta += moveSpeed;
		// Left
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			xDelta -= moveSpeed;
		// Forward
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			zDelta += moveSpeed;
		// Back
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			zDelta -= moveSpeed;
		// Up
		if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.PageUp))
			yDelta += moveSpeed;
		// Down
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.PageDown))
			yDelta -= moveSpeed;
		
		transform.Translate( xDelta * multiplier , yDelta * multiplier , zDelta * multiplier );
	}
}