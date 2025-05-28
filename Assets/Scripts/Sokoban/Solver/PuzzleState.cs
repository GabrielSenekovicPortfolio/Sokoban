using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SokobanSolver;
using UnityEngine.Tilemaps;

//WIP//
public class PuzzleState : IEquatable<PuzzleState>
{
    public Transform Player;
    public Vector3Int Start;
    public List<PushableBlock> Blocks;
    private readonly string hash;

    public PuzzleState(Transform player, Vector3Int start, List<PushableBlock> blocks)
    {
        Player = player;
        Start = start;
        Blocks = new List<PushableBlock>(blocks.Count);
        foreach (var b in blocks)
        {
            Blocks.Add(new PushableBlock { Pos = b.Pos, Pushable = b.Pushable, Trans = b.Trans });
        }
        Blocks.Sort((a, b) => a.Pos.x != b.Pos.x ? a.Pos.x - b.Pos.x : a.Pos.y - b.Pos.y);
        hash = ComputeHash();
    }

    public bool IsGoal(HashSet<Vector3Int> goals)
    {
        return Blocks.All(b => goals.Contains(b.Pos));
    }

    public IEnumerable<(PuzzleState, ISolverAction)> NextStates(
        Tilemap map, Tilemap collisionMap,
        Dictionary<Vector3Int, ActivationField> activators,
        Vector3Int[] dirs)
    {
        var boxSet = new HashSet<Vector3Int>(Blocks.Select(b => b.Pos));

        //Moves
        foreach (var dir in dirs)
        {
            var to = Start + dir;
            if (IsFree(to, boxSet, collisionMap))
            {
                var moveState = new PuzzleState(Player, to, Blocks);
                yield return (moveState, new MoveSolverAction(Player, to));
            }
        }

        //Pushes
        for (int i = 0; i < Blocks.Count; i++)
        {
            var b = Blocks[i];
            if (!b.Pushable) continue;

            foreach (var dir in dirs)
            {
                var target = b.Pos + dir;
                var pushFrom = b.Pos - dir;
                if (Start == pushFrom && IsFree(target, boxSet, collisionMap))
                {
                    var newBlocks = Blocks
                        .Select(ob => new PushableBlock { Pos = ob.Pos, Pushable = ob.Pushable, Trans = ob.Trans })
                        .ToList();
                    newBlocks[i].Pos = target;
                    if (activators.TryGetValue(target, out var af) && af.CanActivate())
                        newBlocks[i].Pushable = false;

                    var nextState = new PuzzleState(Player, b.Pos, newBlocks);
                    var action = new PushSolverAction(Player, b.Trans, target, dir);
                    yield return (nextState, action);
                }
            }
        }
    }

    private HashSet<Vector3Int> GetReachablePositions(Vector3Int start, HashSet<Vector3Int> boxes, Tilemap map, Tilemap collisionMap, Vector3Int[] dirs)
    {
        var visited = new HashSet<Vector3Int> { start };
        var stack = new Stack<Vector3Int>();
        stack.Push(start);
        while (stack.Count > 0)
        {
            var cell = stack.Pop();
            foreach (var d in dirs)
            {
                var neighbor = cell + d;
                if (!visited.Contains(neighbor) && IsFree(neighbor, boxes, collisionMap))
                {
                    visited.Add(neighbor);
                    stack.Push(neighbor);
                }
            }
        }
        return visited;
    }

    private bool IsFree(Vector3Int cell, HashSet<Vector3Int> boxes, Tilemap collisionMap)
    {
        if (boxes.Contains(cell))
        {
            return false;
        }
        var c = collisionMap.GetTile(cell);
        return c == null;
    }

    private string ComputeHash()
    {
        var sb = Start.x + "," + Start.y + ";";
        foreach (var b in Blocks) sb += b.Pos.x + "," + b.Pos.y + ":" + (b.Pushable ? 1 : 0) + ";";
        return sb;
    }

    public bool Equals(PuzzleState other) => other != null && hash == other.hash;
    public override int GetHashCode() => hash.GetHashCode();
}