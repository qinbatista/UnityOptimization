using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class InstanceGameObject : InstanceBase
{
    [SerializeField] SOInstanceConfig _instanceConfig;
    // Vector3 _size;
    List<List<GameObject>> FullGameObjectList = new List<List<GameObject>>();
    static InstanceGameObject _instance;
    public static InstanceGameObject Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public override void Initial()
    {
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _instanceConfig.Size.y; y++)
            for (int x = 0; x < _instanceConfig.Size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_instanceConfig.ECSGameObject, new Vector3(x, y, _instanceConfig.Size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
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
            }
        FullGameObjectList.Add(GameObjectList);
    }

    public override void InstanceUpdate()
    {
        int _objIndex = 0;
        foreach (var theObjList in FullGameObjectList)
        {
            foreach (var item in theObjList)
            {
                int z = _objIndex / ((int)_instanceConfig.Size.x * (int)_instanceConfig.Size.y);
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, z * 5 + Mathf.Sin(Time.time + _objIndex));
                _objIndex++;
            }
        }
    }
}
