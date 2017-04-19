using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public StateStore _StateStore
    {
        get
        {
            return stateStore;
        }
    }
    private StateStore stateStore;

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

    public void ReduceEndGame(float localScore)
    {
        stateStore.SetFloat("localscore", localScore);
    }

    private IEnumerator ReduceEndGameRoutine()
    {
        yield return new WaitForEndOfFrame();
    }


    private IEnumerator LoadScenAsync(string scene)
    {
        AsyncOperation asynct = SceneManager.LoadSceneAsync(scene,LoadSceneMode.Additive);
        while (!asynct.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator UnLoadScenAsync(string scene)
    {
        AsyncOperation asynct = SceneManager.UnloadSceneAsync(scene);
        while (!asynct.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

}

public static class ActionDispatcher
{
    private static bool GetGameStateStore(out GameStateStore gameStateStore)
    {
        gameStateStore = GameObject.FindObjectOfType<GameStateStore>();
        if (gameStateStore != null)
        {
            return true;
        }
        return false;
    }

    public static void DispatchEndGame(float localScore)
    {
        GameStateStore gameStateStore;
        if(GetGameStateStore(out gameStateStore))
        {
            gameStateStore.ReduceEndGame(localScore);
        }
    }

    public static void DispatchGameStart()
    {

    }

    public static void DispatchToMainMenu()
    {

    }

    public static bool TryGetState(out StateStore stateStore)
    {
        GameStateStore gameStateStore = GameObject.FindObjectOfType<GameStateStore>();
        if (gameStateStore != null)
        {
            stateStore = gameStateStore._StateStore;
            return true;
        }
        stateStore = null;
        return false;
    }

}