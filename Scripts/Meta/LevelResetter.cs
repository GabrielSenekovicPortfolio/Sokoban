using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The level resetter takes care of reloading the level and resetting the score.
 * It was made in an effort not to clutter the already slightly cluttered Progression Manager
 */
public class LevelResetter : MonoBehaviour
{
    [SerializeField] ProgressionManager progressionManager;
    public void Restart()
    {
        ScoreManager.Instance.ResetCurrentScore();
        progressionManager.LoadLevel();
    }
}
