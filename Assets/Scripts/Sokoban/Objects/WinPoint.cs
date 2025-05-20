using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/*
 * The Win Point script activates once the puzzle is finished. It takes you to the next level, if there is one
 */
public class WinPoint : MonoBehaviour
{
    [Inject] ProgressionManager progressionManager;

    [SerializeField] Sprite activatedSprite;

    bool activated;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        activated = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        progressionManager.SetWinTile(this);
    }
    public void Activate()
    {
        activated = true;
        spriteRenderer.sprite = activatedSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(activated)
        {
            progressionManager.ProgressToNextLevel();
        }
    }
}
