using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuContainer : MonoBehaviour {

    public Button startButton;
    public Text highscoreText;

	// Use this for initialization
	private void Start () {
        startButton.onClick.AddListener(OnStartButtonClicked);
        highscoreText.text = "highscore : 00000000";

	}
	
	private void OnStartButtonClicked()
    {
        ActionDispatcher.DispatchGameStart();
    }

    public void SetUp(float highscore)
    {
        highscoreText.text = "highscore : "+highscore.ToString("00000000");
    }
}
