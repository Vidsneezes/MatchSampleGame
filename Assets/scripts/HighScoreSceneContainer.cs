using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreSceneContainer : MonoBehaviour {


    public Button continueButton;
    public Text highscoreText;
    public Text localScoreText;

    // Use this for initialization
    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        highscoreText.text = "highscore : 00000000";
        localScoreText.text = "00000000";
    }

    private void OnContinueButtonClicked()
    {
        ActionDispatcher.DispatchToMainMenu();
    }
}
