using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public AudioSource powerUp;
	

	void OnTriggerEnter(Collider collider){
		if(collider.tag == "Player"){
			collider.gameObject.GetComponent<CollisionHandler>().GiveShield(this);
            powerUp.Play();
		}
	}
}
