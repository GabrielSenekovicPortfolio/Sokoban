using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * The Score Manager is used by everything that has a score to add. 
 * It saves some score temporarily for each level and only permanently adds it once you have finished a level.
 */
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    int score = 0;
    int currentScore;
    public void AddScore(int value)
    {
        currentScore += value;
        scoreText.text = (currentScore + score).ToString();
    }
    public void SaveScore()
    {
        //This function saves the temporary score once a level has been finished and you move onto the next
        score += currentScore;
        currentScore = 0;
        scoreText.text = (currentScore + score).ToString();
    }
    public void ResetCurrentScore()
    {
        //This function is used if you reset the level, to reset the temporary score
        currentScore = 0;
        scoreText.text = score.ToString();
    }
    public int GetScore()
    {
        return score;
    }
}
