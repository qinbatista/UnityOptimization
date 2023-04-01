using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
public class InstanceGPUMeshWithJob : InstanceBase
{
    Mesh mesh;
    Material material;
    Vector3 _size;
    int instanceCount;
    Matrix4x4[] matrices;
    RenderParams rp;
    private ComputeBuffer instanceTransformsBuffer;
    private ComputeBuffer indirectArgsBuffer;
    private PositionJob _job;
    private NativeArray<float3> _nativePositions;
    private NativeArray<Matrix4x4> _nativeMatrices;
    static InstanceGPUMeshWithJob _instance;
    public static InstanceGPUMeshWithJob Instance { get => _instance; set => _instance = value; }
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

        _nativePositions = new NativeArray<float3>(instanceCount, Allocator.Persistent);
        _nativeMatrices = new NativeArray<Matrix4x4>(instanceCount, Allocator.Persistent);
        _job = new PositionJob
        {
        };
    }
    public override Vector3 AddAALayer()
    {
        _size.z = _size.z + 1;
        instanceCount = (int)(_size.x * _size.y * _size.z);
        _nativePositions = new NativeArray<float3>(instanceCount, Allocator.Persistent);
        _nativeMatrices = new NativeArray<Matrix4x4>(instanceCount, Allocator.Persistent);
        return _size;
    }
    public override Vector3 ReduceALayer(bool destroyAll = false)
    {
        _size.z = _size.z - 1;
        instanceCount = (int)(_size.x * _size.y * _size.z);
        _nativePositions = new NativeArray<float3>(instanceCount, Allocator.Persistent);
        _nativeMatrices = new NativeArray<Matrix4x4>(instanceCount, Allocator.Persistent);
        return _size;
    }
    public override void InstanceUpdate()
    {
        _job.Matrices = _nativeMatrices;
        _job.Time = Time.time;
        _job.size = _size;
        _job.Schedule(_nativeMatrices.Length, 64).Complete();
        Graphics.RenderMeshInstanced(rp, mesh, 0, _nativeMatrices);
    }
    public void OnDestroy()
    {
        _nativePositions.Dispose();
        _nativeMatrices.Dispose();
    }
}
[BurstCompile]
internal struct PositionJob : IJobParallelFor
{
    public NativeArray<Matrix4x4> Matrices;
    [ReadOnly] public float Time;
    [ReadOnly] public Vector3 size;
    public void Execute(int index)
    {
        int x = (int)(index % size.x);
        int y = (int)(index / size.x) % (int)size.y;
        int z = (int)(index / (size.x * size.y));
        Matrices[index] = Matrix4x4.TRS(new Vector3(x, y, z * 5 + Mathf.Sin(Time + index)), Quaternion.identity, Vector3.one);
    }
}
