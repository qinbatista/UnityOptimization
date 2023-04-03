using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public partial class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SOInstanceConfig _instanceConfig;
    [SerializeField] TextMeshProUGUI _methodName;
    [SerializeField] TextMeshProUGUI _FPS;
    [SerializeField] TextMeshProUGUI _countObjects;
    InstanceBase _instanceBase;
    // int currentLayer = 0;
    void Start()
    {
        _instanceConfig.OpenECS = false;
        SwitchObjectType();
        _instanceConfig.Size = new Vector3(_instanceConfig.Size.x, _instanceConfig.Size.y, 1);
        _instanceBase?.Initial();
    }
    public void AddALayerObjects()
    {
        _instanceBase?.AddAALayer();
    }

    public void SwitchObjectType()
    {
        switch (_instanceConfig.InstanceObjectType)
        {
            case ObjectType.GameObject:
                _instanceBase = InstanceGameObject.Instance;
                print("InstanceGameObject=" + _instanceBase);
                break;
            case ObjectType.GameObjectWithJob:
                _instanceBase = InstanceGameObjectWithJob.Instance;
                print("InstanceGameObjectWithJob=" + _instanceBase);
                break;
            case ObjectType.GPUInstance:
                _instanceBase = InstanceGPUMesh.Instance;
                print("GPUInstance=" + _instanceBase);
                break;
            case ObjectType.GPUInstanceWithJob:
                _instanceBase = InstanceGPUMeshWithJob.Instance;
                print("GPUInstanceWithJob=" + _instanceBase);
                break;
            case ObjectType.ECSInstanceJob:
                print("ECSInstance: Initial success, if no objects, start game again");
                _instanceConfig.OpenECS = true;
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
        _countObjects.text = "Instance:" + (_instanceConfig.Size.x * _instanceConfig.Size.y * _instanceConfig.Size.z).ToString();
        _FPS.text = "FPS:" + ((int)(1.0f / Time.smoothDeltaTime)).ToString();
        _methodName.text = _instanceConfig.InstanceObjectType.ToString();
    }
}
