using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{

	public bool shielded;
	Shield currentShield;
    public GameObject explosion;
    public AudioSource shieldPowerDown;

	int shieldCount = 0;

	Color[] colors = new Color[3];

	void Awake(){

		colors[0] = new Color(255f/255f, 255f/255f, 255f/255f);
		colors[1] = new Color(37f/255f, 147f/255f, 221f/255f);
		colors[2] = new Color(16f/255f, 0f/255f, 255f/255f);


	}

	public void GiveShield(Shield shield){

        if (!shielded)
        {
            shield.transform.parent = transform;
            shield.transform.localPosition = Vector3.zero;
            currentShield = shield;
            shielded = true;
			gameObject.layer = 11; //invincible
			shieldCount++;
        }
        else
        {
			Destroy(shield);
			if(shieldCount < 3){
				shieldCount++;
				currentShield.renderer.material.SetColor("_Color", colors[shieldCount - 1]);
			}
        }
	}

	void CollisionHandle(GameObject obstacle){

		//Destruction of obstacle

		if(EnvironmentManager.instance != null){
				if(obstacle.tag == "CrossBeam")
					EnvironmentManager.instance.crossBeam.Remove(obstacle);
				else 
					EnvironmentManager.instance.obstacleList.Remove(obstacle);
				Destroy(obstacle);
				EnvironmentManager.instance.CreateObstacle();
		}



		if(!shielded){
			if(onDeath != null){
                GameObject currentExplosion = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
				onDeath(gameObject);
			}
		}
		else {
			shieldPowerDown.Play();	
			shieldCount--;

			if(shieldCount == 0){
				Destroy (currentShield.gameObject);
				shielded = false;
				currentShield = null;
				gameObject.layer = 0; //default
			}
			else {
				currentShield.renderer.material.SetColor("_Color", colors[shieldCount - 1]);
			}

		}
	}

	public delegate void DieAction(GameObject gameObject);
	public static event DieAction onDeath;

    void OnCollisionEnter(Collision other)
    {
		if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "CrossBeam")
		{
			CollisionHandle(other.gameObject);
		}
    }

	//Called by the shield, because we set it as a child.
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "CrossBeam")
		{
			CollisionHandle(other.gameObject);
		}
	}
}
