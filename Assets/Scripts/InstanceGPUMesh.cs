using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
public class InstanceGPUMesh : InstanceBase
{
    Mesh mesh;
    Material material;
    Vector3 _size;
    int instanceCount;
    Matrix4x4[] matrices;
    RenderParams rp;
    private ComputeBuffer instanceTransformsBuffer;
    private ComputeBuffer indirectArgsBuffer;
    static InstanceGPUMesh _instance;
    public static InstanceGPUMesh Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public override void Initial(Vector3 size, GameObject gameObject)
    {
        _size = size;
        mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        material = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
        rp = new RenderParams(material);
        instanceCount = (int)(_size.x * _size.y * _size.z);
        matrices = new Matrix4x4[instanceCount];
    }
    public override Vector3 AddAALayer()
    {
        _size.z = _size.z + 1;
        rp = new RenderParams(material);
        instanceCount = (int)(_size.x * _size.y * _size.z);
        matrices = new Matrix4x4[instanceCount];
        return _size;
    }
    public override Vector3 ReduceALayer(bool destroyAll = false)
    {
        _size.z = _size.z - 1;
        rp = new RenderParams(material);
        instanceCount = (int)(_size.x * _size.y * _size.z);
        matrices = new Matrix4x4[instanceCount];
        return _size;
    }
    public override void InstanceUpdate()
    {
        int _objIndex = 0;
        for (int z = 0; z < _size.z; z++)
            for (int y = 0; y < _size.y; y++)
                for (int x = 0; x < _size.x; x++)
                {
                    matrices[_objIndex].SetTRS(new Vector3(x, y, z * 5 + Mathf.Sin(Time.time + _objIndex)), Quaternion.identity, Vector3.one);
                    _objIndex++;
                }
        Graphics.RenderMeshInstanced(rp, mesh, 0, matrices);
    }
}
