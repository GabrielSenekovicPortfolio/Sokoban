using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/*
 * The Gemstone Podium is a script that interacts with the Gemstone to cause effects within the puzzle
 * If all of them are activated, then the Win Point is activated
 * If the scope had been bigger, I could justify an "IPressable" for, perhaps, 
 * buttons that get permanently pressed even if the Gemstone is removed
 */
public class GemstonePodium : SokobanTargetField
{
    [Inject] ScoreManager scoreManager;
    [Inject] ProgressionManager progressionManager;

    [SerializeField] ColorCode color;
    [SerializeField] int score;

    bool activated;
    public void Start()
    {
        progressionManager.AddPodiums(this);
        activated = false;
    }
    public override void Activate(IActivatable activatable)
    {
        base.Activate(activatable);
        activated = true;
        progressionManager.ActivatePodium(color);
        currentActivatable.transform.position = transform.position;
        currentActivatable = null;
        scoreManager.AddScore(score);
    }
}