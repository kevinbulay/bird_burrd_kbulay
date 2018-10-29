using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance;
    
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject counddownPage;
    public Text scoreText;
    
    enum PageState{
        None,
        Start,
        GameOver,
        Countdown
    }
    
    int score = 0;
    bool gameOver = false;
    
    public bool GameOver {get { return gameOver; } }
	// Use this for initialization
	void Awake() {
		if (Instance != null) {
			Destroy(gameObject);
		}
		else {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void OnEnable() {
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
		CountdownText.OnCountdownFinished += OnCountdownFinished;
	}

	void OnDisable() {
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
	}

	void OnCountdownFinished() {
		SetPageState(PageState.None);
		OnGameStarted();
		score = 0;
		gameOver = false;
	}

	void OnPlayerScored() {
		score++;
		scoreText.text = score.ToString();
	}

	void OnPlayerDied() {
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt("HighScore");
		if (score > savedScore) {
			PlayerPrefs.SetInt("HighScore", score);
		}
		SetPageState(PageState.GameOver);
	}
    
    void SetPageState (PageState state) {
        switch (state){
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                counddownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                counddownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                counddownPage.SetActive(false);
                break;
            case PageState.Countdown: 
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                counddownPage.SetActive(true);
                break;
        }
		
	}
    
    public void ConfirmGameOver() {
        //activated when replaybutton is hit
        OnGameOverConfirmed(); //event
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }
    
    public void StartGame() {
        //activated when playbutton is hit
        SetPageState(PageState.Countdown);
    }

}
