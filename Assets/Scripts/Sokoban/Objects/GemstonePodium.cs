using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Gemstone Podium is a script that interacts with the Gemstone to cause effects within the puzzle
 * If all of them are activated, then the Win Point is activated
 * If the scope had been bigger, I could justify an "IPressable" for, perhaps, 
 * buttons that get permanently pressed even if the Gemstone is removed
 */
public class GemstonePodium : SokobanTargetField
{
    [SerializeField] ColorCode color;
    [SerializeField] int score;

    bool activated;
    public void Start()
    {
        ProgressionManager.Instance.AddPodiums(this);
        activated = false;
    }
    public override void Activate(IActivatable activatable)
    {
        base.Activate(activatable);
        activated = true;
        ProgressionManager.Instance.ActivatePodium(color);
        currentActivatable.transform.position = transform.position;
        currentActivatable = null;
        ScoreManager.Instance.AddScore(score);
    }
}