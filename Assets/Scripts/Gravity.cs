using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {
	public float percentage;
	private const float EARTH_GRAVITY = 9.81f;
	// Use this for initialization


	void Start () {
		rigidbody.useGravity = false;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Argument taken as distance/time^2
		rigidbody.AddForce(-Vector3.up * EARTH_GRAVITY *  percentage, ForceMode.Acceleration);
	}
}
