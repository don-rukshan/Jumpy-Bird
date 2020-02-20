using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountDownPage;
    public Text ScoreText;

    enum PageState
    {
        None,
        Start,
        GameOver,
        CountDown
    }

    int score = 0;
    bool GameOver = true;

    public bool gameOver { get { return GameOver; } }
    public int Score { get { return score; } }
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountDownText.OnCountDownFinished += OnCountDownFinished;
        TapController.OnPlayerDied+= OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;

    }
     
    void OnDisable()
    {
        CountDownText.OnCountDownFinished -= OnCountDownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;

    }

    void OnCountDownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        GameOver = false;
    }

    void OnPlayerDied()
    {
        GameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if(score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);

        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        ScoreText.text = score.ToString();
    }
   

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(false);
                break;
            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(false);
                break;
            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                CountDownPage.SetActive(false);
                break;
            case PageState.CountDown:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(true);
                break;

        }
    }

    public void ConfirmGameOver()
    {
        //activated when replay button is hit
         OnGameOverConfirmed(); //event
        ScoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        //activated when play button is hit
        SetPageState(PageState.CountDown);
    }
}
