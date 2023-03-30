using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public partial class DynamicObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _gameObject;
    [SerializeField] TextMeshProUGUI _FPS;
    [SerializeField] TextMeshProUGUI _countObjects;
    [SerializeField] Vector3 _size;
    [SerializeField] ObjectType _objectType;
    List<List<GameObject>> FullGameObjectList = new List<List<GameObject>>();
    int angle;
    // int currentLayer = 0;
    void Start()
    {
        AddALayerObjects();
    }
    public void AddALayerObjects()
    {
        switch (_objectType)
        {
            case ObjectType.GameObject:
                GameObjectAddALayerObjects();
                break;
            case ObjectType.GPUInstance:
                GPUInstanceAddALayerObjects();
                break;

        }
    }
    public void ReduceALayerObjects()
    {
        switch (_objectType)
        {
            case ObjectType.GameObject:
                GameObjectReduceALayerObjects();
                break;
            case ObjectType.GPUInstance:
                GPUInstanceReduceALayerObjects();
                break;
        }
    }
    public void SwitchObjectType()
    {
        switch (_objectType)
        {
            case ObjectType.GameObject:
                GameObjectReduceALayerObjects();
                break;
            case ObjectType.GPUInstance:
                GPUInstanceReduceALayerObjects();
                break;
        }
    }
    void Update()
    {
        switch (_objectType)
        {
            case ObjectType.GameObject:
                GameObjectUpdate();
                break;
            case ObjectType.GPUInstance:
                GPUInstanceUpdate();
                break;
        }
        DisplayInfo();
    }
    void DisplayInfo()
    {
        _countObjects.text = "Instance:" + (_size.x * _size.y * _size.z).ToString();
        _FPS.text = "FPS:" + ((int)(1.0f / Time.smoothDeltaTime)).ToString();
    }
}
