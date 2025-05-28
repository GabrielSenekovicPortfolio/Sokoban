using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ActivationField : MonoBehaviour, IActivator
{
    protected List<Type> IActivatableTypes = new List<Type>();
    protected bool canActivate;
    public virtual void Activate(IActivatable activatable)
    {
        activatable.Activate();
    }

    public bool CanActivate() => canActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
          for(int i = 0; i < IActivatableTypes.Count; i++)
        {
            if (collision.gameObject.TryGetComponent(IActivatableTypes[i], out Component comp))
            {
                IActivatable activatable = comp as IActivatable;
                Activate(activatable);
            }
        }
        
    }
}
