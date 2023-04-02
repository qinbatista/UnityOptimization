using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class ECSSystemBase : SystemBase
{
    int count = 10;
    int index = 0;
    protected override void OnUpdate()
    {
        // EntityQuery entityQuery = GetEntityQuery(typeof(AuthoringTag));
        SpawnSpawnerComponent  authoringDataComponent = SystemAPI.GetSingleton<SpawnSpawnerComponent>();
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        if(index < count)
        {
            // EntityManager.Instantiate(authoringDataComponent.entity);
            Entity entity =  ecb.Instantiate(authoringDataComponent.entity);
            ecb.SetComponent(entity, new AuthoringTagComponent
            {
                _id = index
            });
            index++;
        }
        foreach (ECSAspect _ECSAspect in SystemAPI.Query<ECSAspect>())
        {
            _ECSAspect.SetPosition(SystemAPI.Time.ElapsedTime);
        }
    }
}
