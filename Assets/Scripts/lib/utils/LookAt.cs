using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
	public Transform Target;
	// public Vector3Constraints RotationConstraints;
	public Vector3Constraints Invert;
	public Vector3 Offset = Vector3.zero;

	public Vector3Constraints OverridesEnabled;
	public Vector3 Overrides;

	[Range(0, 1)]
	public float Smoothing = .2f;

	// Use this for initialization
	void Start()
	{
		if ( !Target )
			Target = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(Target);

		// float lerpSpeed = 1 + ( 1 / Smoothing );
		// Vector3 newRotation = Vector3.RotateTowards( transform.forward , Target.position , 1f , 0f );

		Vector3 newRotation = new Vector3(transform.eulerAngles.x + Offset.x, 180 + transform.eulerAngles.y + Offset.y, transform.eulerAngles.z + Offset.z);

		newRotation.x *= Invert.x ? -1 : 1;
		newRotation.y *= Invert.y ? -1 : 1;
		newRotation.z *= Invert.z ? -1 : 1;

		if ( OverridesEnabled.x )
			newRotation.x = Overrides.x;
		if ( OverridesEnabled.y )
			newRotation.y = Overrides.y;
		if ( OverridesEnabled.z )
			newRotation.z = Overrides.z;

		transform.eulerAngles = newRotation;
		// transform.eulerAngles = Vector3.Lerp( transform.eulerAngles , newRotation , Time.deltaTime * lerpSpeed );
	}
}
