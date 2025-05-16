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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(adjacentPushableObject != null)
            {
                StartPushing();
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            StopPushing();
        }
    }
}
