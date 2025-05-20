using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/*
 * The Platform script is activated by Gemstones when a Gemstone of corresponding color is moved to its Podium
 * It cannot be walked on normally, but it can once activated
 * If the scope had been bigger, it could warrant an "IPlatform", for perhaps, platforms that deactivate and reactivate based on a timer
 */
public class Platform : MonoBehaviour
{
    [Inject] ProgressionManager progressionManager;

    [HideInInspector] public string enumTypeName;
    [HideInInspector] public int enumValueIndex;

    [SerializeField] Sprite sprite;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        progressionManager.AddPlatform(this);
    }
    public void Activate()
    {
        spriteRenderer.sprite = sprite;
        boxCollider.enabled = false;
    }

    public int GetEnumValue() => enumValueIndex;
}
