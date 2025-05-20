using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanTargetField : ActivationField
{
    protected SokobanPushable currentActivatable;

    private void Awake()
    {
        IActivatableTypes.Add(typeof(SokobanPushable));
    }

    public override void Activate(IActivatable activatable)
    {
        //Seeing as this is a SokobanTarget, and the class itself is supposed to only interact with a given list of classes,
        //it has to be asserted here
        base.Activate(activatable);
        Debug.Assert(activatable is SokobanPushable);
        currentActivatable = activatable as SokobanPushable;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentActivatable == collision.gameObject)
        {
            currentActivatable = null;
        }
    }
}
