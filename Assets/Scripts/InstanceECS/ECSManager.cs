using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
public class ECSManager : MonoBehaviour
{
    [SerializeField] SOInstanceConfig _instanceConfig;
    public Vector3 Size;
    public SOInstanceConfig InstanceConfig { get => _instanceConfig; }
}
public class InstanceECSManagerBaker : Baker<ECSManager>
{
    public override void Bake(ECSManager authoring)
    {
        Entity _entityManager = GetEntity(authoring.gameObject, TransformUsageFlags.ManualOverride);
        Entity _entity = GetEntity(authoring.InstanceConfig.ECSGameObject, TransformUsageFlags.Dynamic);
        AddComponent(_entityManager, new SpawnSpawnerComponent
        {
            entity = _entity,
            size = authoring.Size,
            openECS = authoring.InstanceConfig.OpenECS
        });
    }
}
public struct SpawnSpawnerComponent : IComponentData
{
    public Entity entity;
    public Vector3 size;
    public bool openECS;
}