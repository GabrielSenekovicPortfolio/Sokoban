using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Player Push script deals with grabbing onto pushable objects, in this case only Gemstones
 * As well as dealing with getting the direction of the push
 * Since moving when pushing goes under movement, its in Player Movement and not here
 * If other entities could push, then it could justify an "IPusher" interface
 */
public class PlayerPush : MonoBehaviour
{
    GameObject pushedObject;
    GameObject adjacentPushableObject;
    Transform originalParent;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(adjacentPushableObject != null)
            {
                StartPush();
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            DropObject();
        }
    }
    public bool GetPushDirection(out Vector2 direction)
    {
        if(pushedObject != null)
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
    public void StartPush()
    {
        pushedObject = adjacentPushableObject;
        originalParent = pushedObject.transform.parent;
        pushedObject.transform.parent = transform;

        Vector2 position = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
        position = pushedObject.transform.position;
        pushedObject.transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);

        pushedObject.GetComponent<Gemstone>().StartPushing(this);
    }

    public bool IsPushing()
    {
        return pushedObject != null;
    }
    public void DropObject()
    {
        if(pushedObject != null)
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
        if(collision.gameObject.TryGetComponent(out Gemstone gemstone))
        {
            adjacentPushableObject = gemstone.gameObject;
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
