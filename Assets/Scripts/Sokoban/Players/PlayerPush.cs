using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Player Push script deals with grabbing onto pushable objects, in this case only Gemstones
 * As well as dealing with getting the direction of the push
 * Since moving when pushing goes under movement, its in Player Movement and not here
 */
public class PlayerPush : PusherAbstract
{
    Timer pushTimer;

    private new void Awake()
    {
        base.Awake();
        pushTimer = new Timer(0.2f, StartPushing);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Space) && pushingSequence == null)
        {
            pushTimer.Tick();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            pushTimer.Reset();
            StopPushing();
        }
    }
}
