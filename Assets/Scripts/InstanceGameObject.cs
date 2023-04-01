using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class InstanceGameObject : InstanceBase
{
    Vector3 _size;
    GameObject _gameObject;
    List<List<GameObject>> FullGameObjectList = new List<List<GameObject>>();
    static InstanceBase _instance;
    public static InstanceBase Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public override void Initial(Vector3 size, GameObject gameObject)
    {
        _size = size;
        _gameObject = gameObject;
        AddAALayer();
    }
    public override Vector3 AddAALayer()
    {
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _size.y; y++)
            for (int x = 0; x < _size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_gameObject, new Vector3(x, y, _size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
            }
        FullGameObjectList.Add(GameObjectList);
        _size.z++;
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
                _size.z--;
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
        int _objIndex = 0;
        foreach (var theObjList in FullGameObjectList)
        {
            foreach (var item in theObjList)
            {
                int z = _objIndex / ((int)_size.x * (int)_size.y);
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, z * 5 + Mathf.Sin(Time.time + _objIndex));
                _objIndex++;
            }
        }
    }
}
