using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

[InitializeOnLoad]
[ExecuteInEditMode]
static class TilemapReparent
{
    static TilemapReparent()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    static void OnHierarchyChanged()
    {
        if (Application.isPlaying) return;
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        Transform root = prefabStage != null
            ? prefabStage.prefabContentsRoot.transform
            : null;
        Tilemap[] tilemaps = root != null
            ? root.GetComponentsInChildren<Tilemap>(true)
            : Object.FindObjectsOfType<Tilemap>(true);
        if (tilemaps.Length == 0) return;

        TilemapManager mgrComp;
        if (root != null)
        {
            mgrComp = root.GetComponentInChildren<TilemapManager>();
            if (mgrComp == null)
            {
                var go = new GameObject("TilemapManager", typeof(TilemapManager));
                go.transform.SetParent(root, false);
                mgrComp = go.GetComponent<TilemapManager>();
                Undo.RegisterCreatedObjectUndo(go, "Create Prefab TilemapManager");
            }
        }
        else
        {
            mgrComp = Object.FindObjectOfType<TilemapManager>();
            if (mgrComp == null)
            {
                var go = new GameObject("TilemapManager", typeof(TilemapManager));
                Undo.RegisterCreatedObjectUndo(go, "Create TilemapManager");
                mgrComp = go.GetComponent<TilemapManager>();
            }
        }

        var mgrTransform = mgrComp.transform;

        foreach (var tilemap in tilemaps)
        {
            if (tilemap.transform.parent != mgrTransform)
            {
                Undo.SetTransformParent(tilemap.transform, mgrTransform, "Reparent Tilemap");
            }
        }

        if (root == null)
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}