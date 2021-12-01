using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get { return instance; } }
    public Transform popupText;
    public TMPro.TextMeshProUGUI scoreText;

    private int score;
    private int mainScore;
    private Dictionary<string, int> scoreMultiplierDic;
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
        mainScore = PlayerPrefs.GetInt("mainScore", 0);
        scoreText.text = mainScore.ToString();
        knifeManager = FindObjectOfType<KnifeManager>();
        knifeManager.OnGameStateChanged += KnifeManager_OnGameStateChanged;
    }

    private void KnifeManager_OnGameStateChanged(GameManager.GameState gState)
    {
        if ( gState == GameManager.GameState.Finish )
        {
            PlayerPrefs.SetInt("mainScore", mainScore + score);
        }
    }

    private void Start()
    {
        scoreMultiplierDic = new Dictionary<string, int>();
        scoreMultiplierDic.Add("1x", 1);
        scoreMultiplierDic.Add("2x", 2);
        scoreMultiplierDic.Add("3x", 3);
        scoreMultiplierDic.Add("4x", 4);
        scoreMultiplierDic.Add("6x", 6);
        scoreMultiplierDic.Add("10x", 10);
        scoreMultiplierDic.Add("50x", 50);
        scoreText.text = mainScore.ToString();
    }
    public void PopupText(Vector3 position, string text)
    {
        Transform popup = Instantiate(popupText, position + Vector3.left, Quaternion.identity);
        popup.GetComponent<PopupText>().displayText = text;
        score++;
        scoreText.text = (mainScore + score).ToString();
    }
    public void CalculateScore(string scoreMultiplier)
    {
        score = score * scoreMultiplierDic[scoreMultiplier];
    }
    public int GetScore() => score;

    private void OnDisable()
    {
        knifeManager.OnGameStateChanged -= KnifeManager_OnGameStateChanged;
    }
    
}
