using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
public readonly partial struct ECSAspect : IAspect
{
    private readonly Entity _entity;
    private readonly RefRO<AuthoringTagComponent> _authoringTag;
    private readonly RefRW<AuthoringDataComponent> _authoringData;
    public void SetPosition(double time)
    {
        _authoringData.ValueRW.speed = _authoringTag.ValueRO._id * time;
    }
}