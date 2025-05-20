using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SokobanPushable : PushableAbstract, IActivatable
{
    public virtual void Activate()
    {
        pushable = false;
    }

    public void Deactive()
    {
        pushable = true;
    }
}
