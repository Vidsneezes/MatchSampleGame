using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateStore : MonoBehaviour {

    #region PlayerPrefConsts
    public const string FLOAT_HIGHSCORE = "FLOAT_HIGHSCORE";
    public const string FLOAT_LOCALSCORE = "LOCAL_SCORE";
    #endregion

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

    public void ChangeScene(SceneChangeStruct sceneChangeStruct)
    {
        StartCoroutine(SceneChangeRoutine(sceneChangeStruct));
    }

    private IEnumerator SceneChangeRoutine(SceneChangeStruct sceneChangeStruct)
    {
        if (sceneChangeStruct.unloadScene != "")
        {
            yield return StartCoroutine(UnLoadScenAsync(sceneChangeStruct.unloadScene));
        }
        if (sceneChangeStruct.loadScene != "")
        {
            yield return StartCoroutine(LoadScenAsync(sceneChangeStruct.loadScene));
        }
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

public struct SceneChangeStruct
{
    public string unloadScene;
    public string loadScene;
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
            float highScore = 0;
            gameStateStore._StateStore.GetFloat(GameStateStore.FLOAT_HIGHSCORE, out highScore);
            gameStateStore._StateStore.SetFloat(GameStateStore.FLOAT_LOCALSCORE, localScore);
            if(localScore > highScore)
            {
                gameStateStore._StateStore.SetFloat(GameStateStore.FLOAT_HIGHSCORE, localScore);
            }
            SceneChangeStruct sceneChangeStruct;
            sceneChangeStruct.unloadScene = "MainScene";
            sceneChangeStruct.loadScene = "HighscoreScene";
            gameStateStore.ChangeScene(sceneChangeStruct);
        }
    }

    public static void DispatchGameStart()
    {
        GameStateStore gameStateStore;
        if (GetGameStateStore(out gameStateStore))
        {
        }
    }

    public static void DispatchToMainMenu()
    {
        GameStateStore gameStateStore;
        if (GetGameStateStore(out gameStateStore))
        {
        }
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