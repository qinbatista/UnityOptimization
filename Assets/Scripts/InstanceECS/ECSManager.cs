using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
public class ECSManager : MonoBehaviour
{
    Vector3 _size;
    [SerializeField] GameObject _ECSGameObject;
    static ECSManager _instance;
    public static ECSManager Instance { get => _instance; set => _instance = value; }
    public GameObject ECSGameObject { get => _ECSGameObject; }

    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public void Initial(Vector3 size, GameObject gameObject)
    {
        _ECSGameObject = gameObject;
        print("ECSGameObject=" + _ECSGameObject);
    }
    public Vector3 AddAALayer()
    {
        return new Vector3(0, 0, 0);
    }
    public Vector3 ReduceALayer(bool destroyAll = false)
    {
        return new Vector3(0, 0, 0);
    }
    public void InstanceUpdate()
    {

    }
}
public class InstanceECSManagerBaker : Baker<ECSManager>
{
    public override void Bake(ECSManager authoring)
    {
        Entity _entityManager = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
        Entity _entity = GetEntity(authoring.ECSGameObject, TransformUsageFlags.Dynamic);
        AddComponent(_entityManager, new SpawnSpawnerComponent
        {
            entity = _entity,
        });
    }
}
public struct SpawnSpawnerComponent : IComponentData
{
    public Entity entity;
}