using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * The Player Movement script deals with everything that has to do with moving the player
 * Currently there is nothing else that moves, otherwise it could warrant a "Player Controller" and an "IMovement"
 */
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;

    public bool canMove;

    Rigidbody2D body;
    PlayerPush push;
    bool solving = false;

    private void Awake()
    {
        canMove = true;
        body = GetComponent<Rigidbody2D>();
        push = GetComponent<PlayerPush>();
    }

    private void Update()
    {
        GetDirectionFromInput(out Vector2 movementDirection);
        ModifyDirection(ref movementDirection);
        Move(movementDirection);
#if UNITY_EDITOR
        if(!solving && Input.GetKeyDown(KeyCode.F9))
        {
            solving = true;
            FindObjectOfType<TilemapManager>().HasTilemap("Objects", out var tilemap);
            FindObjectOfType<TilemapManager>().HasTilemap("Collision", out var collisionTilemap);
            var blocks = FindObjectsOfType<SokobanPushable>().ToList();
            var fields = FindObjectsOfType<GemstonePodium>();
            var goals = fields.Select(p => p.transform.position.ToV3Int());
            SokobanSolver solver = new SokobanSolver(tilemap, collisionTilemap, goals, fields);
            var actionList = solver.Solve(transform, transform.position.ToV3Int(), blocks);
            Debug.Log("Success! steps: " + actionList.Count);
            ActionPlayer.PlayOut(actionList);
        }
#endif
    }
    public void GetDirectionFromInput(out Vector2 movementDirection)
    {
        movementDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementDirection.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementDirection.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDirection.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementDirection.x = 1;
        }
    }
    public void ModifyDirection(ref Vector2 movementDirection)
    {
        if (push.IsPushing())
        {
            push.GetPushDirection(out Vector2 pushDirection);
            movementDirection.x = movementDirection.x == pushDirection.x ? pushDirection.x : 0;
            movementDirection.y = movementDirection.y == pushDirection.y ? pushDirection.y : 0;

            //The code below allows you to also pull, and not only push. 
            //This might be useful in case I decide to have an object like that
            //movementDirection.x *= Mathf.Abs(pushDirection.x);
            //movementDirection.y *= Mathf.Abs(pushDirection.y);

            movementDirection.Normalize();
        }
    }
    public void Move(Vector2 movementDirection)
    {
        if (!canMove) return;
        body.velocity = movementDirection.normalized * movementSpeed;
    }
}
