using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public static class VectorExtensions
{
    public static Vector3 Round(this Vector3 V3)
    {
        return new Vector3(Mathf.RoundToInt(V3.x), Mathf.RoundToInt(V3.y), Mathf.RoundToInt(V3.z));
    }
    public static Vector2Int ToV2Int(this Vector2 V2)
    {
        return new Vector2Int(Mathf.RoundToInt(V2.x), Mathf.RoundToInt(V2.y));
    }
    public static Vector2Int ToV2Int(this Vector3 V3)
    {
        return new Vector2Int(Mathf.RoundToInt(V3.x), Mathf.RoundToInt(V3.y));
    }
    public static Vector3Int ToV3Int(this Vector3 V3)
    {
        return new Vector3Int(Mathf.RoundToInt(V3.x), Mathf.RoundToInt(V3.y), Mathf.RoundToInt(V3.z));
    }
    public static Vector3 ToV3(this Vector2Int V2)
    {
        return new Vector3(V2.x, V2.y, 0);
    }
}
