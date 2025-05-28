using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//WIP//
public class AStar<TState, TAction> where TState : PuzzleState
{
    public delegate bool GoalTest(TState state);
    public delegate IEnumerable<(TState state, TAction action)> NeighborFunc(TState state);

    HashSet<Vector3Int> goals;

    public AStar(HashSet<Vector3Int> goals)
    {
        this.goals = goals;
    }

    public List<TAction> Search(
        TState start,
        GoalTest isGoal,
        NeighborFunc neighbors)
    {
        var open = new PriorityQueue<TState>();
        var gScore = new Dictionary<TState, int>();
        var cameFrom = new Dictionary<TState, (TState prev, TAction action)>();

        gScore[start] = 0;
        open.Enqueue(start, Heuristic(start));
        int i = 0;
        while (open.Count > 0)
        {
            i++;
            var current = open.Dequeue();
            if (isGoal(current))
            {
                return ReconstructPath(cameFrom, current);
            }

            int currentG = gScore[current];
            foreach (var (next, action) in neighbors(current))
            {
                int tentative = currentG + 1;
                if (!gScore.TryGetValue(next, out int existing) || tentative < existing)
                {
                    cameFrom[next] = (current, action);
                    gScore[next] = tentative;
                    open.Enqueue(next, tentative + Heuristic(next));
                }
            }
        }
        Debug.Log(i + " amount of loops");
        Debug.LogError("A* Search: no solution found");
        return null;
    }

    List<TAction> ReconstructPath(
        Dictionary<TState, (TState prev, TAction action)> cameFrom,
        TState current)
    {
        var path = new List<TAction>();
        while (cameFrom.TryGetValue(current, out var info))
        {
            path.Insert(0, info.action);
            current = info.prev;
        }
        return path;
    }
    int Heuristic(TState s)
    {
        int h = 0;
        foreach (var b in s.Blocks)
        {
            int best = int.MaxValue;
            foreach (var g in goals)
            {
                int d = Mathf.Abs(b.Pos.x - g.x) + Mathf.Abs(b.Pos.y - g.y);
                best = Mathf.Min(best, d);
            }
            h += best;
        }
        return h;
    }

}