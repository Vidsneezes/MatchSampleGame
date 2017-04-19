using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateStore : MonoBehaviour {

    #region PlayerPrefConsts
    private const string FLOAT_HIGHSCORE = "FLOAT_HIGHSCORE";
    private const string FLOAT_LOCALSCORE = "LOCAL_SCORE";
    #endregion

    #region actions
    public const string END_GAME = "END_GAME";
    private const string MAIN_MENU = "MAIN_MENU";
    private const string GAME = "GAME";
    private const string HIGHSCORE = "HIGHSCORE";
    #endregion
    public string state;


	// Use this for initialization
	void Start () {
        ToMainMenu();
	}
	
    public void ReduceState(string action)
    {

    }

    private IEnumerator ReduceStateRoutine(string action)
    {
        switch (state)
        {
            case MAIN_MENU:
                MainMenuContainer mainMenuContainer = GameObject.FindObjectOfType<MainMenuContainer>();
                mainMenuContainer.onStartButtonClicked += ToGame;
                float highscore = PlayerPrefs.GetFloat(FLOAT_HIGHSCORE);
                break;
            case GAME:
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                gameManager.onTimeDone += EndGame;
                break;
            case HIGHSCORE:
                HighScoreSceneContainer highScoreContainer = GameObject.FindObjectOfType<HighScoreSceneContainer>();
                highScoreContainer.onContinueButtonClicked += ToMainMenu;
                break;
        }
        state = "-";
        yield return new WaitForEndOfFrame();
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

    private void EndGame()
    {

    }

}

public static class ActionDispatcher
{
    public static string START_GAME = "START_GAME";
    public static string END_GAME = "END_GAME";
    public static string GO_TO_MAINMENU = "GO_TO_MAINMENU";

    public static void Dispatch(string action)
    {
        GameStateStore gameStateStore = GameObject.FindObjectOfType<GameStateStore>();
        if(gameStateStore != null)
        {
            gameStateStore.ReduceState(action);
        }
    }

}

