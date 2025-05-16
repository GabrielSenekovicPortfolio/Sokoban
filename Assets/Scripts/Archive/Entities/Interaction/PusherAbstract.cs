using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherAbstract : MonoBehaviour, IPusher
{
    protected PushableAbstract pushedObject;
    protected PushableAbstract adjacentPushableObject;
    protected Transform originalParent;
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
        pushedObject = adjacentPushableObject;
        originalParent = pushedObject.transform.parent;
        pushedObject.transform.parent = transform;

        Vector2 position = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
        position = pushedObject.transform.position;
        pushedObject.transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);

        pushedObject.GetComponent<PushableAbstract>().StartPushing(this);
    }
    public virtual void StopPushing()
    {
        if (pushedObject != null)
        {
            Vector2 position = pushedObject.transform.position;
            pushedObject.transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);

            pushedObject.transform.parent = originalParent;
            pushedObject = null;
            originalParent = null;
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
