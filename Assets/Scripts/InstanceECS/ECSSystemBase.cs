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
        //get the reference of the component
        RefRO<SpawnSpawnerComponent> authoringDataComponentRO = SystemAPI.GetSingletonRW<SpawnSpawnerComponent>();
        //don't run code if the openECS is false
        if (authoringDataComponentRO.ValueRO.openECS == false) return;

        // EntityQuery entityQuery = GetEntityQuery(typeof(AuthoringTag));

        //get the value from the component
        SpawnSpawnerComponent authoringDataComponent = SystemAPI.GetSingleton<SpawnSpawnerComponent>();
        Vector3 value = authoringDataComponentRO.ValueRO.size;


        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        //give the count value
        if (count != (int)(value.x * value.y * value.z)) count = (int)(value.x * value.y * value.z);

        //generate the entity
        while (index != count)
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

        // find all entities with the ECSAspect and LocalTransform, you can modify the value
        Entities.ForEach((ref LocalTransform _localTransform, ref ECSAspect _ECSAspect) =>
        {
            _localTransform = _ECSAspect.SetPosition(SystemAPI.Time.ElapsedTime, _localTransform, authoringDataComponent.size);
        }).ScheduleParallel();
    }
}
