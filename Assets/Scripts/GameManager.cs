using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    KnifeManager knifeManager;
    private void Awake()
    {
        if ( instance != null && instance != this )
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public enum GameState
    {
        Waiting,
        Playing,
        Finish,
        GameOver,
        Challange
    }

    public GameState gameState;


    public void OnClassicButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnChallangeButtonClick()
    {
        SceneManager.LoadScene(2);
        gameState = GameState.Challange;
    }

    public void BeSubs()
    {
        knifeManager = FindObjectOfType<KnifeManager>();
        knifeManager.OnGameStateChanged += KnifeManager_OnGameStateChanged;
    }

    private void KnifeManager_OnGameStateChanged(GameState gState)
    {
        gameState = gState;
        //If game state is Finish or Game Over then unsubscribe 
        if ( gState != GameState.Playing )
        {
            knifeManager.OnGameStateChanged -= KnifeManager_OnGameStateChanged;
        }
    }
}
