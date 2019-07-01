using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour {

	public float Speed;
	public float THRUST_FORCE = 30f;
	public bool UseThrust;
	public bool ResetPos;

    public const float BOOST = 20f;


	public ParticleSystem leftThruster;
    public ParticleSystem leftFrontThruster;
    public ParticleSystem rightFrontThruster;
	public ParticleSystem rightThruster;
    public ParticleSystem topThurster;
    public ParticleSystem backThurster;

	Vector3 originalPos;
	Quaternion originalRotation;
	void Awake(){
		originalPos = transform.position;
		originalRotation = transform.rotation;
	}
	void Start () {
		rigidbody.velocity = transform.forward * Speed;
		Input.ResetInputAxes();
	}

	bool goLeft = false;
	bool goRight = false;
	bool goDown = false;
    bool goForward = false;

	// Update is called once per frame
	
	void Update () {
        Speed += Time.deltaTime * 0.35f;
		//Change thruster state based on input.
		if(Input.GetButton("Left")){
			if(!goLeft){
				goLeft = true; 
				//To go left use right thruster and vice versa
				rightThruster.Play();
                rightFrontThruster.Play();
			}
		}
		else if(UseThrust && goLeft){
			goLeft = false;
			rightThruster.Stop();
            rightFrontThruster.Stop();
		}
		if(Input.GetButton("Right")){
			if(!goRight){
				goRight = true;
				leftThruster.Play();
                leftFrontThruster.Play();
                
			}
		}
		else if(UseThrust && goRight){
			goRight = false;
			leftThruster.Stop();
            leftFrontThruster.Stop();
		}

		if(Input.GetButton("Down")){
			if(!goDown){
				goDown = true;
				topThurster.Play();
            }
		}
		else if(goDown){
			goDown = false;
			topThurster.Stop();
        }

        if (Input.GetButton("Jump"))
        {
            if (!goForward)
            {
                goForward = true;
                backThurster.Play();
            }
        }
        else if (goForward)
        {
            goForward = false;
            backThurster.Stop();
        }

        if(Input.GetButtonDown("Boost"))
        {
            Speed += BOOST;
        }
        if (Input.GetButtonUp("Boost"))
        {
            Speed -= BOOST;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Speed += BOOST*6;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Speed -= BOOST*6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rigidbody.AddForce(0, 1000f, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Speed += BOOST * 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Speed -= BOOST * 6;
        }
#endif

        /*if(Input.GetButtonDown("Jump")){
			ApplyThrust(transform.up, THRUST_FORCE * 20f);
		}*/

		if(!UseThrust){
			if (transform.position.x > EnvironmentManager.instance.targetLane) 
			{
				transform.position = new Vector3(transform.position.x -.5f, transform.position.y, transform.position.z);
				
				rightThruster.Stop();
			}
			if (transform.position.x < EnvironmentManager.instance.targetLane)
			{
				transform.position = new Vector3(transform.position.x + .5f, transform.position.y, transform.position.z);
				
				leftThruster.Stop();
			}
		}


		if(ResetPos){
			if(transform.position.z > 60){
				transform.position = originalPos;
				transform.rotation = originalRotation;
			}
		}
	}

	void FixedUpdate()
	{

		//Note you can use both thrusters at once.
		if(goLeft){
			if(UseThrust){
				ApplyThrust(transform.right * -1f, THRUST_FORCE);
			}
			else {
				EnvironmentManager.instance.targetLane = (int)EnvironmentManager.instance.targetLane - 1;
				Debug.Log("TargetLane: " + EnvironmentManager.instance.targetLane);
				goLeft = false;
			}

		}
		
		if(goRight){
			if(UseThrust){
				ApplyThrust(transform.right, THRUST_FORCE);
			}
			else {
				EnvironmentManager.instance.targetLane = (int)EnvironmentManager.instance.targetLane + 1;
				goRight = false;
				Debug.Log("TargetLane: " + EnvironmentManager.instance.targetLane);
			}


		}

		if(goDown)
		{
			ApplyThrust(transform.up * -1f, THRUST_FORCE);
		}
		//Moves the body forward regardless of other factors.
		MoveForward();
	}


	//Applies a force. Adds up if button is held.
	private void ApplyThrust(Vector3 direction, float THRUST){
		rigidbody.AddForce(direction * THRUST);
	}

	private void MoveForward(){
		Vector3 desiredVelocity = rigidbody.velocity;
		desiredVelocity.z = Speed;
		rigidbody.velocity = desiredVelocity;
		//rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
	}
}
