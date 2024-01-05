using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * The Gemstone, being the only pushable object, deals with activation and being pushed on its own
 * If there were more, I could justify an "IPushable" and an "IActivatable"
 * The Gemstone is a pushable object that must be pushed onto objects with the "Gemstone Podium" script
 */
public class Gemstone : MonoBehaviour
{
    [SerializeField] ColorCode color;

    Light2D myLight;
    PlayerPush playerPush;

    bool pushable;
    public bool IsPushable() => pushable;
    public ColorCode GetColorCode() => color;
    private void Awake()
    {
        myLight = GetComponentInChildren<Light2D>();
        myLight.enabled = false;
        pushable = true;
    }
    public void StartPushing(PlayerPush playerPush) //Can take different playerpush in case there are multiple players
    {
        this.playerPush = playerPush;
    }
    public void Activate()
    {
        playerPush.DropObject();
        playerPush = null;
        pushable = false;
        StartGlowing();
    }

    void StartGlowing()
    {
        myLight.enabled = true;
    }
}
