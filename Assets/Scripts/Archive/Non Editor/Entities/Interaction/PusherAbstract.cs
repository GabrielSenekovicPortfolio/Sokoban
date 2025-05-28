using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PusherAbstract : MonoBehaviour, IPusher
{
    protected PushableAbstract pushedObject;
    protected PushableAbstract adjacentPushableObject;
    protected Sequence pushingSequence;

    PlayerMovement movement;
    public void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
    public bool IsPushing() => pushedObject != null;
    public bool GetPushDirection(out Vector2 direction)
    {
        if (pushedObject != null)
        {
            direction = pushedObject.transform.position - transform.position;
            direction.Normalize();
            direction.x = Mathf.RoundToInt(direction.x);
            direction.y = Mathf.RoundToInt(direction.y);
            return true;
        }
        direction = Vector2.zero;
        return false;
    }
    public virtual void StartPushing()
    {
        if (adjacentPushableObject == null) return;

        movement.canMove = false;

        pushedObject = adjacentPushableObject;

        var dir = (pushedObject.transform.position - transform.position).Round().normalized;

        pushedObject.GetComponent<PushableAbstract>().StartPushing(this);

        pushingSequence = DOTween.Sequence();
        pushingSequence.Append(transform.DOMove(transform.position + dir, 0.2f));
        pushingSequence.Join(pushedObject.transform.DOMove(pushedObject.transform.position + dir, 0.2f));
        pushingSequence.OnComplete(() =>
        {
            pushingSequence = null;
        });
        pushingSequence.Play();
    }
    public virtual void StopPushing()
    {
        if (pushedObject != null)
        {
            pushedObject = null;
            movement.canMove = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushableAbstract gemstone) && gemstone.IsPushable())
        {
            adjacentPushableObject = gemstone;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == adjacentPushableObject)
        {
            adjacentPushableObject = null;
        }
    }
}
