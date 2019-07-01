using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public float HOVER_FORCE;
	public float HoverHeight;
	public float DampingFactor = .3f;
	int layerMask;

	void Start(){
		int layer = LayerMask.NameToLayer("Platform");
		layerMask = 1 << layer;
	}

	Vector3 targetUp;
	float slerpRate;
	//interpolate faster when connected to a platform
	public float slerpRateSurface = 3f;
	public float slerpRateStabilize = 1f;

	public bool AutoRotate;


	void FixedUpdate(){
		RaycastHit info;
		if(Physics.Raycast(transform.position, -Vector3.up, out info, HoverHeight, layerMask)){
			//if raycast is hitting, target up = normal of colliding surface
			targetUp = info.normal;
			slerpRate = slerpRateSurface;
			//The desired height - distance to floor. Should be positive
			float error = HoverHeight - info.distance;
			//Scale force by amount of error. The more we're off, the faster we want to get to our desired height
			float scaledForce = error * HOVER_FORCE; // kg/ms/s^2
			//To tone down bounciness, reduce force a little depending on direction.
			float currentDirection = rigidbody.velocity.y < 0 ? -1f : 1f;
			float smooth = currentDirection * scaledForce * DampingFactor;

			rigidbody.AddForce(Vector3.up * (scaledForce - smooth));
		}
		else {
			slerpRate = slerpRateStabilize;
			//if raycast is not hitting, target up = world.up
			targetUp = Vector3.up;
		}
	}

	void LateUpdate(){
		//Note that transform.up is being passed in as a parameter, so latest vector is always interpolated
		//to target vector.
		if(AutoRotate)
			transform.up = Vector3.Slerp(transform.up, targetUp, Time.deltaTime * slerpRate);
	}
}
