using UnityEngine;
using System.Collections;

public class ChangeSceneButton : MonoBehaviour {

	public string level;

	void OnClick(){
			Application.LoadLevel(level);
	}
}
