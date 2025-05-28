using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionPlayer
{
    public static void PlayOut(List<ISolverAction> actions)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Pause();
        foreach(var action in actions)
        {
            action.Perform(sequence);
        }
        sequence.Play();
    }
}
