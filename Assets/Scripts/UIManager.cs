using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject finishPanel;
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI calculateScoreText;
    public TMPro.TextMeshProUGUI totalScoreText;
    KnifeManager knifeManager;
    private void Awake()
    {
        knifeManager = FindObjectOfType<KnifeManager>();
        knifeManager.OnGameStateChanged += KnifeManager_OnGameStateChanged;
    }

    private void KnifeManager_OnGameStateChanged(GameManager.GameState gameState)
    {
        if ( gameState == GameManager.GameState.Finish )
        {
            finishPanel.SetActive(true);
            int calculatedScore = ScoreManager.Instance.GetScore();
            calculateScoreText.DOText("+" + calculatedScore.ToString(), 3, false, ScrambleMode.Numerals)
                .OnComplete(() => totalScoreText.text = PlayerPrefs.GetInt("mainScore").ToString());
        }
        else if ( gameState == GameManager.GameState.GameOver )
        {
            gameOverPanel.SetActive(true);
        }
    }
    public void OnTab()
    {
        GameManager.Instance.gameState = GameManager.GameState.Playing;
        playPanel.SetActive(false);
    }
    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        knifeManager.OnGameStateChanged -= KnifeManager_OnGameStateChanged;
    }
}
