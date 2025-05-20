using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivator 
{
    public bool CanActivate();
    public void Activate(IActivatable activatable);
}
