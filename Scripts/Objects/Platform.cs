using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Platform script is activated by Gemstones when a Gemstone of corresponding color is moved to its Podium
 * It cannot be walked on normally, but it can once activated
 * If the scope had been bigger, it could warrant an "IPlatform", for perhaps, platforms that deactivate and reactivate based on a timer
 */
public class Platform : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] ColorCode color;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        ProgressionManager.Instance.AddPlatform(this);
    }
    public void Activate()
    {
        spriteRenderer.sprite = sprite;
        boxCollider.enabled = false;
    }

    public ColorCode GetColorCode()
    {
        return color;
    }
}
