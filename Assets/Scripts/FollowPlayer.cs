using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	/*
	 * Possible Cameras:
	 * Fixed: Ignore player position except for z axis. Camera maintains original rotation.
	 * Centered.  Same as fixed, but always look at player. If player goes above camera, we will see below him.
	 * Above: Stays certain amount of degrees above player, looks at player. This will keep player in middle of screen.
	 */
	enum CameraMode{
		Fixed,
		Centered,
		Above
	}

	CameraMode cameraMode = CameraMode.Above;

	[SerializeField]
	private Transform target;

	public Transform Target
	{
		get {return target;}
		set 
		{
			target = value;
			transform.LookAt(target);
		}
	}

	public float Distance;
	public float YOffset;
	Vector3 OffSetVector;
	public float XRotation;

	void Start(){
		if(target != null)
			transform.LookAt(target);
		OffSetVector.y = YOffset;
	}


	void LateUpdate (){

		if(target == null){
			Debug.LogWarning("Target not set");
			this.enabled = false;
			return;
		}

		Vector3 desiredPosition;

		switch(cameraMode)
		{
			case CameraMode.Fixed:
			{
				desiredPosition = transform.position;
				desiredPosition.z = target.position.z - Distance;
				transform.position = desiredPosition;
				break;
			}

			case CameraMode.Centered:
			{
				desiredPosition = transform.position;
				desiredPosition.z = target.position.z - Distance;
				transform.position = desiredPosition;
				transform.LookAt(target);

				break;
			}

			case CameraMode.Above:
			{
				desiredPosition = PositionAboutTarget(target.position, XRotation, Distance);
				transform.position = desiredPosition;
				transform.LookAt(target);
				transform.position += OffSetVector;
				break;
			}
		}
	}

	//Take a vector certain distance from origin, rotate it. Return this vector + target position
	Vector3 PositionAboutTarget(Vector3 targetPosition, float rotationX, float distance){
		Vector3 distFromOrigin = new Vector3(0f, 0f, -distance);
		//take euler angles rotation from target.
		Quaternion rotation = Quaternion.Euler(rotationX, 0f, 0f);
		//Add clean rotated vector to position of target
		return targetPosition + rotation * distFromOrigin;
	}

}
 