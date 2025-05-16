using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * The Gemstone, being the only pushable object, deals with activation and being pushed on its own
 * The Gemstone is a pushable object that must be pushed onto objects with the "Gemstone Podium" script
 */
public class Gemstone : SokobanPushable
{
    [SerializeField] ColorCode color;

    Light2D myLight;

    public ColorCode GetColorCode() => color;
    private void Awake()
    {
        myLight = GetComponentInChildren<Light2D>();
        myLight.enabled = false;
        pushable = true;
    }
    public override void Activate()
    {
        base.Activate();
        playerPush.StopPushing();
        playerPush = null;
        StartGlowing();
    }

    void StartGlowing()
    {
        myLight.enabled = true;
    }
}
