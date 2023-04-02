using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ECSSystemBase : SystemBase
{
    int count = -1;
    int index = 0;
    protected override void OnUpdate()
    {
        // EntityQuery entityQuery = GetEntityQuery(typeof(AuthoringTag));
        SpawnSpawnerComponent authoringDataComponent = SystemAPI.GetSingleton<SpawnSpawnerComponent>();
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        if (count == -1)
            count = (int)(authoringDataComponent.size.x * authoringDataComponent.size.y * authoringDataComponent.size.z);
        if (index < count)
        {
            // EntityManager.Instantiate(authoringDataComponent.entity);
            Entity entity = ecb.Instantiate(authoringDataComponent.entity);
            ecb.SetComponent(entity, new AuthoringTagComponent
            {
                _id = index
            });
            index++;
        }
        //find all entities with the ECSAspect and LocalTransform, you can't modify the value but about to call them
        // foreach ((ECSAspect _ECSAspect,LocalTransform _localTransform) in SystemAPI.Query<ECSAspect, LocalTransform>())
        // {
        //     _ECSAspect.SetPosition(SystemAPI.Time.ElapsedTime,_localTransform);
        // }
        //find all entities with the ECSAspect and LocalTransform, you can modify the value
        Entities.ForEach((ref LocalTransform _localTransform, ref ECSAspect _ECSAspect) =>
        {
            _localTransform = _ECSAspect.SetPosition(SystemAPI.Time.ElapsedTime, _localTransform, authoringDataComponent.size);
        }).ScheduleParallel();
    }
}
