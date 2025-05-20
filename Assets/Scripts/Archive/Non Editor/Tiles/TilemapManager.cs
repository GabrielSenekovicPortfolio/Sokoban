using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance { get; private set; }

    public SerializableDictionary<string, Tilemap> tileMaps = new SerializableDictionary<string, Tilemap>();

#if UNITY_EDITOR
    private void OnEnable()
    {
        try
        {
            Instance = this;

            UpdateDictionary();

            EditorApplication.hierarchyChanged += OnHierarchyChanged;

            if (!Application.isPlaying)
            {
                EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
            }
        }
        catch
        {
            Debug.Log("Something went wrong with enabling tilemap manager");
        }
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
    }

    private void OnHierarchyChanged()
    {
        UpdateDictionary();
    }
    private void HandlePlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Instance = null;
        }
    }
#endif

    private void Awake()
    {
        Instance = this;
    }

    void UpdateDictionary()
    {
        try
        {
            tileMaps.Clear();
            var tileMapsList = GetComponentsInChildren<Tilemap>().ToList();
            foreach (var tilemap in tileMapsList)
            {
                tileMaps.Add(tilemap.name, tilemap);
            }
        }
        catch
        {

        }
    }
    public void ProcessRuleTileGameObjects()
    {
        foreach(var tilemap in tileMaps.Values)
        {
            Vector3 offset = tilemap.tileAnchor;
            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(position);

                if (tile is RuleTile)
                {
                    GameObject instantiatedGameObject = tilemap.GetInstantiatedObject(position);

                    if (instantiatedGameObject != null)
                    {
                        instantiatedGameObject.transform.position = new Vector3(position.x, position.y) + offset + transform.position;
                    }
                }
            }
        }
    }

    private void Start()
    {
        UpdateDictionary();
    }
    public bool HasTilemap(string tilemapName, out Tilemap tilemap)
    {
        tilemapName.ToLower();
        return tileMaps.TryGetValue(tilemapName, out tilemap);
    }
    public bool AddTilemap(string tilemapName, out Tilemap tilemap)
    {
        tilemap = null;
        try
        {
            tilemapName.ToLower();
            if (HasTilemap(tilemapName, out tilemap))
            {
                return true;
            }
            else
            {
                GameObject newTilemapGO = new GameObject(tilemapName);
                newTilemapGO.transform.parent = transform;

                tilemap = newTilemapGO.AddComponent<Tilemap>();
                newTilemapGO.AddComponent<TilemapRenderer>();
                tileMaps.Add(tilemapName, tilemap);
            }
            return false;
        }
        catch
        {
            Debug.Log("Something went wrong with adding a tilemap");
            return false;
        }
    }
    public bool GetTilemapsBelowLayer(int layer, out List<Tilemap> tilemaps)
    {
        tilemaps = new List<Tilemap>();

        for(int i = 0; i < this.tileMaps.Count; i++)
        {
            if (this.tileMaps.ElementAt(i).Value.GetComponent<TilemapRenderer>().sortingOrder < layer)
            {
                tilemaps.Add(tileMaps.ElementAt(i).Value);
            }
        }
        return tilemaps.Count > 0;
    }
}
