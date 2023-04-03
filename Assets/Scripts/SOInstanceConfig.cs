using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceConfig", menuName = "ScriptableObjects/InstanceConfig", order = 1)]
public class SOInstanceConfig : ScriptableObject
{
    [SerializeField] GameObject _ECSGameObject;
    [SerializeField] Vector3 _size;
    [SerializeField] ObjectType _instanceObjectType;
    bool _openECS = false;
    public GameObject ECSGameObject { get => _ECSGameObject; }
    public Vector3 Size { get => _size; set => _size = value; }
    public ObjectType InstanceObjectType { get => _instanceObjectType; }
    public bool OpenECS { get => _openECS; set => _openECS = value; }
    private void OnEnable()
    {
        if (_instanceObjectType == ObjectType.ECSInstanceJob) _openECS = true;
        else _openECS = false;
    }

}
