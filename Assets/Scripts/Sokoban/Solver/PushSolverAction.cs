using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//WIP//
public struct PushSolverAction : ISolverAction
{
    public Vector3Int To;
    public Vector3Int Direction;
    public ActionType Type() => ActionType.Push;

    Transform pusher;
    Transform blockTrans;

    public PushSolverAction(Transform pusher, Transform blockTrans, Vector3Int To, Vector3Int Dir)
    {
        this.pusher = pusher;
        this.blockTrans = blockTrans;
        this.To = To;
        this.Direction = Dir;
    }
    public void Perform(Sequence seq)
    {
        seq.Append(pusher.DOMove(To - Direction, 1f));
        seq.Join(blockTrans.DOMove(To, 1f));
    }
}
