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
        StateStore stateStore;
        float highScore = 0;
        float localScore = 0;
        if(ActionDispatcher.TryGetState(out stateStore))
        {
            stateStore.GetFloat(GameStateStore.FLOAT_HIGHSCORE, out highScore);
            stateStore.GetFloat(GameStateStore.FLOAT_LOCALSCORE, out localScore);
        }
        highscoreText.text = "highscore : "+highScore.ToString("00000000");
        localScoreText.text = localScore.ToString("00000000");
    }

    private void OnContinueButtonClicked()
    {
        ActionDispatcher.DispatchToMainMenu();
    }
}
