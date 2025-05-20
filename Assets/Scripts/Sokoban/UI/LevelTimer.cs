using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

/*
 * The Level Timer script handles the level timer. When the timer runs out, you get a game over
 * There is currently one set readonly value that it always sets to
 * This could be made variable, if different levels should have different difficulty
 */
public class LevelTimer : MonoBehaviour
{
    [Inject] ScoreManager scoreManager;

    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] GameOverManager gameOverManager;
    readonly int timerStartValue = 300;

    float currentTimerValue;

    private void Awake()
    {
        currentTimerValue = 0;
    }

    private void Update()
    {
        currentTimerValue -= Time.deltaTime;
        timer.text = Mathf.RoundToInt(currentTimerValue).ToString();
        if(currentTimerValue <= 0)
        {
            OnTimeRunOut();
        }
    }
    public void SaveTimerScore()
    {
        scoreManager.AddScore((int)currentTimerValue);
    }
    public void ResetTimer()
    {
        currentTimerValue = timerStartValue;
    }
    public void OnTimeRunOut()
    {
        ResetTimer();
        gameOverManager.GameOver();
    }
}
