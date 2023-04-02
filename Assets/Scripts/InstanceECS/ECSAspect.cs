using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public readonly partial struct ECSAspect : IAspect
{
    private readonly Entity _entity;
    private readonly RefRW<AuthoringDataComponent> _authoringData;
    private readonly RefRO<AuthoringTagComponent> _authoringTag;


    public LocalTransform SetPosition(double time, LocalTransform localTransform, Vector3 size)
    {
        int index = _authoringTag.ValueRO._id;
        // Vector3 size = _authoringData.ValueRW.size;
        // Debug.Log("size=" + size + "index=" + index);
        int x = (int)(index % size.x);
        int y = (int)(index / size.x) % (int)size.y;
        int z = (int)(index / (size.x * size.y));
        localTransform = localTransform.WithPosition(new Vector3(x, y, z * 5 + Mathf.Sin((float)time + index)));
        return localTransform;
    }
}