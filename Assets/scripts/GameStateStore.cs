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
	}
	
    public void ReduceState(string action)
    {
        StartCoroutine(ReduceStateRoutine(action));
    }

    private IEnumerator ReduceStateRoutine(string action)
    {
        switch (action)
        {
            case ActionDispatcher.START_GAME:
                break;
            case ActionDispatcher.GO_TO_MAINMENU:
                break;
            case ActionDispatcher.END_GAME:
                break;
        }
        yield return new WaitForEndOfFrame();
    }
}

public static class ActionDispatcher
{
    public const string START_GAME = "START_GAME";
    public const string END_GAME = "END_GAME";
    public const string GO_TO_MAINMENU = "GO_TO_MAINMENU";

    public static void Dispatch(string action)
    {
        GameStateStore gameStateStore = GameObject.FindObjectOfType<GameStateStore>();
        if(gameStateStore != null)
        {
            gameStateStore.ReduceState(action);
        }
    }

}