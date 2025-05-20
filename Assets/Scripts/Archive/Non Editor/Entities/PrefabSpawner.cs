using UnityEngine;
using Zenject;

public class PrefabSpawner : MonoInstaller
{
    public override void InstallBindings()
    {
    }
    public bool SpawnPrefab<T>(T prefab, out T spawnedEntity) where T : Object
    {
        spawnedEntity = Container.InstantiatePrefab(prefab, transform).GetComponent<T>();
        return spawnedEntity != null;
    }
    public bool SpawnPrefab(GameObject prefab, out GameObject spawnedEntity)
    {
        spawnedEntity = Container.InstantiatePrefab(prefab, transform);
        return spawnedEntity != null;
    }
}