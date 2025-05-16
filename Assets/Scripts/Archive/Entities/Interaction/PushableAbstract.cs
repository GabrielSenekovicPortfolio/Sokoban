using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PushableAbstract : MonoBehaviour, IPushable
{
    protected bool pushable;
    protected PusherAbstract playerPush;

    public void StartPushing(PusherAbstract playerPush) //Can take different playerpush in case there are multiple players
    {
        this.playerPush = playerPush;
    }
    public bool IsPushable() => pushable;
}
