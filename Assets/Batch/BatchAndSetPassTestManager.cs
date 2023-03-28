using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class BatchAndSetPassTestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _cube;
    [SerializeField] TextMeshProUGUI _FPS;
    [SerializeField] TextMeshProUGUI _countObjects;
    [SerializeField] Vector3 _size;
    List<GameObject> GameObjectList = new List<GameObject>();
    int angle;
    void Start()
    {
        for (int z = 0; z < _size.z; z++)
            for (int y = 0; y < _size.y; y++)
                for (int x = 0; x < _size.x; x++)
                {
                    GameObject cubeInstance = Instantiate(_cube, new Vector3(x, y, z), Quaternion.identity);
                    GameObjectList.Add(cubeInstance);
                }
        _countObjects.text = "Instance:" + (_size.x * _size.y * _size.z).ToString();

    }
    // Update is called once per frame
    void Update()
    {
        int _objIndex = 0;
        foreach (var item in GameObjectList)
        {
            int z = _objIndex / ((int)_size.x * (int)_size.y);
            item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, z * 5 + Mathf.Sin(Time.time + _objIndex));
            _objIndex++;
        }
        _FPS.text = "FPS:" + ((int)(1.0f / Time.smoothDeltaTime)).ToString();

    }
}
