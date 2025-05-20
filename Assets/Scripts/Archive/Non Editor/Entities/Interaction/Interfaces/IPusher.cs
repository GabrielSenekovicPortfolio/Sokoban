using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPusher
{
    public void StartPushing();
    public bool IsPushing();
    public void StopPushing();
}
