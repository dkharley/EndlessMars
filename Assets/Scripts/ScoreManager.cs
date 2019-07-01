using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance;

	public int bestScore;
	public int currentScore;

	void Awake(){
		if(instance == null)
			instance = this;
		else {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}

	public void LoadScore(){

	}

	public void UpdateScore(float distanceTraveled){
		int score = (int)distanceTraveled;
		if(score > bestScore)
			bestScore = score;
		currentScore = score;
	}

	public void SubmitScore(){
		KongregateAPI.instance.SubmitStats("Distance", currentScore);
	}
}
