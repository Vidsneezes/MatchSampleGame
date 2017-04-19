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
        float highScore = 0;
        StateStore stateStore;
        if(ActionDispatcher.TryGetState(out stateStore))
        {
            stateStore.GetFloat(GameStateStore.FLOAT_HIGHSCORE, out highScore);
        }
        highscoreText.text = "highscore : "+  highScore.ToString("00000000");

	}
	
	private void OnStartButtonClicked()
    {
        ActionDispatcher.DispatchGameStart();
    }
  
}
