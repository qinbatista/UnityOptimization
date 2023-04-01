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
    Vector3 _size;
    GameObject _gameObject;
    List<List<GameObject>> FullGameObjectList = new List<List<GameObject>>();
    private GameObjectPositionJob _job;
    static InstanceGameObjectWithJob _instance;
    public TransformAccessArray _transformAccessArray;
    public static InstanceGameObjectWithJob Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public override void Initial(Vector3 size, GameObject gameObject)
    {
        _size = size;
        _gameObject = gameObject;
        _transformAccessArray = new TransformAccessArray((int)(_size.x * _size.y * _size.z));
        _job = new GameObjectPositionJob();
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _size.y; y++)
            for (int x = 0; x < _size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_gameObject, new Vector3(x, y, _size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
                _transformAccessArray.Add(cubeInstance.transform);
            }
        FullGameObjectList.Add(GameObjectList);
    }
    public override Vector3 AddAALayer()
    {
        _size.z = _size.z + 1;
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _size.y; y++)
            for (int x = 0; x < _size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_gameObject, new Vector3(x, y, _size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
                _transformAccessArray.Add(cubeInstance.transform);
            }
        FullGameObjectList.Add(GameObjectList);
        return _size;
    }
    public override Vector3 ReduceALayer(bool destroyAll = false)
    {
        if (destroyAll)
        {
            foreach (var item in FullGameObjectList)
            {
                foreach (var item2 in item)
                {
                    Destroy(item2);
                }
            }
            FullGameObjectList.Clear();
            _size.z = 0;
            return _size;
        }
        else
        {
            if (_size.z > 1)
            {
                _size.z = _size.z - 1;
                foreach (var item in FullGameObjectList[(int)_size.z])
                {
                    Destroy(item);
                }
                FullGameObjectList.RemoveAt((int)_size.z);
            }
            return _size;
        }
    }
    public override void InstanceUpdate()
    {
        _job.Time = Time.time;
        _job.deltaTime = Time.deltaTime;
        _job.size = _size;
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
