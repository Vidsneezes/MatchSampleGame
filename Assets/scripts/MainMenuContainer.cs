using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuContainer : MonoBehaviour {

    public Button startButton;
    public Text highscoreText;

    public Action onStartButtonClicked;

	// Use this for initialization
	private void Start () {
        startButton.onClick.AddListener(OnStartButtonClicked);
        highscoreText.text = "highscore : 00000000";

	}
	
	private void OnStartButtonClicked()
    {
        if(onStartButtonClicked != null)
        {
            onStartButtonClicked();
        }
    }
}
