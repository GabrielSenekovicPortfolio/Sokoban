using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

//WIP//
public class SokobanSolver
{
    Tilemap objectsTilemap;
    Tilemap boundsTilemap;
    HashSet<Vector3Int> goals;
    Dictionary<Vector3Int, ActivationField> activators;
    readonly Vector3Int[] dirs = new[] {
        new Vector3Int(1,0,0), new Vector3Int(-1,0,0),
        new Vector3Int(0,1,0), new Vector3Int(0,-1,0)
    };

    public SokobanSolver(Tilemap tilemap, Tilemap collisionTilemap,
        IEnumerable<Vector3Int> goalPositions,
        IEnumerable<ActivationField> activationFields)
    {
        objectsTilemap = tilemap;
        boundsTilemap = collisionTilemap;
        goals = new HashSet<Vector3Int>(goalPositions);
        activators = new Dictionary<Vector3Int, ActivationField>();
        foreach (var af in activationFields)
        {
            var cell = tilemap.WorldToCell(af.transform.position);
            activators[cell] = af;
        }
    }

    public List<ISolverAction> Solve(Transform player, Vector3Int playerStart, List<SokobanPushable> blocks_in)
    {
        var blocks = new List<PushableBlock>();
        for(int i = 0; i < blocks_in.Count; i++)
        {
            blocks.Add(new PushableBlock
            {
                Pos = blocks_in[i].transform.position.ToV3Int(),
                Pushable = blocks_in[i].IsPushable(),
                Trans = blocks_in[i].transform
            });
        }
        var start = new PuzzleState(player, playerStart, blocks);
        var aStar = new AStar<PuzzleState, ISolverAction>(goals);
        return aStar.Search(start, 
            s => s.IsGoal(goals),
            s => s.NextStates(objectsTilemap, boundsTilemap, activators, dirs));
    }

    private List<ISolverAction> ConstructActionPath(Dictionary<PuzzleState, (PuzzleState prev, ISolverAction action)> cf, PuzzleState cur)
    {
        var path = new List<ISolverAction>();
        while (cf.TryGetValue(cur, out var info))
        {
            path.Insert(0, info.action);
            cur = info.prev;
        }
        return path;
    }
    public class PushableBlock 
    { 
        public Vector3Int Pos; 
        public bool Pushable;
        public Transform Trans;
    }
}
