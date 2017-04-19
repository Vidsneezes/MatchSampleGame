using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreSceneContainer : MonoBehaviour {


    public Button startButton;
    public Text highscoreText;
    public Text localScoreText;

    public Action onContinueButtonClicked;

    // Use this for initialization
    private void Start()
    {
        startButton.onClick.AddListener(OnContinueButtonClicked);
        highscoreText.text = "highscore : 00000000";
        localScoreText.text = "00000000";
    }

    private void OnContinueButtonClicked()
    {
        if (onContinueButtonClicked != null)
        {
            onContinueButtonClicked();
        }
    }
}
