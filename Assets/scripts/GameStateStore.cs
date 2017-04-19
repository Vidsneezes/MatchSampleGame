using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateStore : MonoBehaviour {

    private const string MAIN_MENU = "MAIN_MENU";
    private const string GAME = "GAME";
    private const string HIGHSCORE = "HIGHSCORE";

    public string state;

	// Use this for initialization
	void Start () {
        ToMainMenu();
	}
	
	// Update is called once per frame
	void Update () {
        if(state != "-")
        {
            ReduceState();
        }
	}

    public void ReduceState()
    {
        switch (state)
        {
            case MAIN_MENU:
                MainMenuContainer mainMenuContainer = GameObject.FindObjectOfType<MainMenuContainer>();
                mainMenuContainer.onStartButtonClicked += ToGame;
                break;
            case GAME:
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                gameManager.onTimeDone += ToHighscore;
                break;
            case HIGHSCORE:
                HighScoreSceneContainer highScoreContainer = GameObject.FindObjectOfType<HighScoreSceneContainer>();
                highScoreContainer.onContinueButtonClicked += ToMainMenu;
                break;
        }
        state = "-";
    }

    private void ToMainMenu()
    {
        state = MAIN_MENU;
    }

    private void ToGame()
    {
        state = GAME;
    }

    private void ToHighscore()
    {
        state = HIGHSCORE;
    }

}
