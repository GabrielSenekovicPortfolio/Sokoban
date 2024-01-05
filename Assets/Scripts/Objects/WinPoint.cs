using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Win Point script activates once the puzzle is finished. It takes you to the next level, if there is one
 */
public class WinPoint : MonoBehaviour
{
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
        ProgressionManager.Instance.SetWinTile(this);
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
            ProgressionManager.Instance.ProgressToNextLevel();
        }
    }
}
