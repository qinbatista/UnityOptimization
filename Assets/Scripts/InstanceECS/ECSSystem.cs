using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct ECSSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
    }

    void OnDestroy(ref SystemState state)
    {
    }
    void OnUpdate(ref SystemState state)
    {
        //get the reference of the component
        // RefRO<SpawnSpawnerComponent> authoringDataComponentRO = SystemAPI.GetSingletonRW<SpawnSpawnerComponent>();
        //don't run code if the openECS is false
        // if (authoringDataComponentRO.ValueRO.openECS == false) return;
        // new SetPositionJob
        // {
        //     time = (float)SystemAPI.Time.ElapsedTime,
        //     size = authoringDataComponentRO.ValueRO.size
        // }.ScheduleParallel();
    }
}
[BurstCompile]
public partial struct SetPositionJob : IJobEntity
{
    public float time;
    public Vector3 size;
    public void Execute(ref LocalTransform localTransform, ref ECSAspect _ECSAspect)
    {
        localTransform = _ECSAspect.SetPosition(time, localTransform, size);
    }
}
