using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public partial class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _gameObject;
    [SerializeField] TextMeshProUGUI _FPS;
    [SerializeField] TextMeshProUGUI _countObjects;
    [SerializeField] Vector3 _size;
    [SerializeField] ObjectType _objectType;
    [SerializeField] Subsystem _sceneManager;
    int angle;
    InstanceBase _instanceBase;
    // int currentLayer = 0;
    void Start()
    {
        SwitchObjectType();
        _instanceBase?.Initial(_size, _gameObject);
    }
    public void AddALayerObjects()
    {
        _size = _instanceBase.AddAALayer();
    }
    public void ReduceALayerObjects()
    {
        _size = _instanceBase.ReduceALayer();
    }
    public void SwitchObjectType()
    {
        switch (_objectType)
        {
            case ObjectType.GameObject:
                _instanceBase = InstanceGameObject.Instance;
                print("GameObject=" + _instanceBase);
                break;
            case ObjectType.GameObjectWithJob:
                _instanceBase = InstanceGameObjectWithJob.Instance;
                print("GameObject=" + _instanceBase);
                break;
            case ObjectType.GPUInstance:
                _instanceBase = InstanceGPUMesh.Instance;
                print("GPUInstance=" + _instanceBase);
                break;
            case ObjectType.GPUInstanceWithJob:
                _instanceBase = InstanceGPUMeshWithJob.Instance;
                print("GPUInstanceWithJob=" + _instanceBase);
                break;
            case ObjectType.ECSInstance:
                // _instanceBase = ECSManager.Instance;
                print("ECSInstance=" + _instanceBase);
                break;
        }
    }
    void Update()
    {
        _instanceBase?.InstanceUpdate();
        DisplayInfo();
    }
    void DisplayInfo()
    {
        _countObjects.text = "Instance:" + (_size.x * _size.y * _size.z).ToString();
        _FPS.text = "FPS:" + ((int)(1.0f / Time.smoothDeltaTime)).ToString();
    }
}
