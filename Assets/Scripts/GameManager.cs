using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	public UIPanel gameOverPanel;
	public UIPanel PausePanel;
	public UILabel BestDistance;
	public UILabel CurrentDistance;
	public GameObject ScoringPrefab;

	bool gameOver = false;
	bool paused = false;

	void Start(){
		PausePanel.gameObject.SetActive(false);
		if(ScoreManager.instance == null){
			Instantiate(ScoringPrefab, Vector3.zero, Quaternion.identity);
		}
	}

	void OnEnable(){
		CollisionHandler.onDeath += HandleDeath;
	}

	void OnDisable(){
		CollisionHandler.onDeath -= HandleDeath;
	}

	void HandleDeath(GameObject gameObject){
		Destroy(gameObject);
		gameOver = true;
		EnvironmentManager.instance.enabled = false;

		BestDistance.text = ScoreManager.instance.bestScore.ToString();
		CurrentDistance.text = ScoreManager.instance.currentScore.ToString();
		ScoreManager.instance.SubmitScore();

		gameOverPanel.gameObject.SetActive(true);
	}


	void Restart(){
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			PauseToggle();
		}
	}

	//keep this here until we get a gamestate manager
	void SetPauseState(bool pause){
		if(pause){
			Time.timeScale = 0f;
		}
		else {
			Time.timeScale = 1f;
		}
	}
	
	void PauseToggle(){
		if(gameOver)
			return;
		paused = !paused;
		SetPauseState(paused);
		PausePanel.gameObject.SetActive(paused);
	}
}
