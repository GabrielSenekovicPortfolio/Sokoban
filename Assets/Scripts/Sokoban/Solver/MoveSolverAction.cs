using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//WIP//
public struct MoveSolverAction : ISolverAction
{
    public Vector3Int Position;
    public ActionType Type() => ActionType.Move;

    Transform trans;

    public MoveSolverAction(Transform trans, Vector3Int Pos)
    {
        this.trans = trans;
        this.Position = Pos;
    }
    public void Perform(Sequence seq)
    {
        seq.Append(trans.DOMove(Position, 1f));
    }
}
