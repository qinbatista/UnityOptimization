using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public partial class DynamicObjectPool : MonoBehaviour
{
    public void GameObjectAddALayerObjects()
    {
        List<GameObject> GameObjectList = new List<GameObject>();
        for (int y = 0; y < _size.y; y++)
            for (int x = 0; x < _size.x; x++)
            {
                GameObject cubeInstance = Instantiate(_gameObject, new Vector3(x, y, _size.z), Quaternion.identity, this.transform);
                GameObjectList.Add(cubeInstance);
            }
        _size.z++;
        FullGameObjectList.Add(GameObjectList);
    }
    public void GameObjectReduceALayerObjects(bool destroyAll = false)
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
            return;
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
        }
    }
    public void GameObjectUpdate()
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
