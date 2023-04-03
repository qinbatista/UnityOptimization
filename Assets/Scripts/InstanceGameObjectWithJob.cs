using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class InstanceGameObjectWithJob : InstanceBase
{
    [SerializeField] SOInstanceConfig _instanceConfig;
    List<List<GameObject>> FullGameObjectList = new List<List<GameObject>>();
    private GameObjectPositionJob _job;
    static InstanceGameObjectWithJob _instance;
    public TransformAccessArray _transformAccessArray;
    public static InstanceGameObjectWithJob Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public override void Initial()
    {
        _transformAccessArray = new TransformAccessArray((int)(_instanceConfig.Size.x * _instanceConfig.Size.y * _instanceConfig.Size.z));
        _job = new GameObjectPositionJob();
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _instanceConfig.Size.y; y++)
            for (int x = 0; x < _instanceConfig.Size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_instanceConfig.ECSGameObject, new Vector3(x, y, _instanceConfig.Size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
                _transformAccessArray.Add(cubeInstance.transform);
            }
        FullGameObjectList.Add(GameObjectList);
    }
    public override void AddAALayer()
    {
        _instanceConfig.Size = new Vector3(_instanceConfig.Size.x, _instanceConfig.Size.y, _instanceConfig.Size.z + 1);
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _instanceConfig.Size.y; y++)
            for (int x = 0; x < _instanceConfig.Size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_instanceConfig.ECSGameObject, new Vector3(x, y, _instanceConfig.Size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
                _transformAccessArray.Add(cubeInstance.transform);
            }
        FullGameObjectList.Add(GameObjectList);
    }

    public override void InstanceUpdate()
    {
        _job.Time = Time.time;
        _job.deltaTime = Time.deltaTime;
        _job.size = _instanceConfig.Size;
        _job.Schedule(_transformAccessArray).Complete();
    }
    void OnDestroy()
    {
        if (FullGameObjectList.Count > 0)
            _transformAccessArray.Dispose();
    }
}
[BurstCompile]
internal struct GameObjectPositionJob : IJobParallelForTransform
{
    [ReadOnly] public float Time;
    [ReadOnly] public float deltaTime;
    [ReadOnly] public Vector3 size;
    public void Execute(int index, TransformAccess transform)
    {
        int x = (int)(index % size.x);
        int y = (int)(index / size.x) % (int)size.y;
        int z = (int)(index / (size.x * size.y));
        transform.position = new Vector3(x, y, z * 5 + Mathf.Sin(Time + index));
    }
}
