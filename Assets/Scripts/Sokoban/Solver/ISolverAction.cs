using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ActionType { Move, Push }
public interface ISolverAction
{
    public ActionType Type();
    public void Perform(Sequence seq);
}