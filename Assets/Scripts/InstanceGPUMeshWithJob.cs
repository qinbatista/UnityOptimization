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
    [SerializeField] SOInstanceConfig _instanceConfig;
    Mesh mesh;
    Material material;
    int instanceCount;
    Matrix4x4[] matrices;
    RenderParams rp;
    private ComputeBuffer instanceTransformsBuffer;
    private ComputeBuffer indirectArgsBuffer;
    private GPUPositionJob _job;
    private NativeArray<Matrix4x4> _nativeMatrices;
    static InstanceGPUMeshWithJob _instance;
    public static InstanceGPUMeshWithJob Instance { get => _instance; set => _instance = value; }
    public virtual void Awake()
    {
        if (_instance != null) { Destroy(gameObject); } else { _instance = this; }
    }
    public override void Initial()
    {
        mesh = _instanceConfig.ECSGameObject.GetComponent<MeshFilter>().sharedMesh;
        material = _instanceConfig.ECSGameObject.GetComponent<MeshRenderer>().sharedMaterial;
        rp = new RenderParams(material);
        instanceCount = (int)(_instanceConfig.Size.x * _instanceConfig.Size.y * _instanceConfig.Size.z);

        _nativeMatrices = new NativeArray<Matrix4x4>(instanceCount, Allocator.Persistent);
        _job = new GPUPositionJob
        {
        };
    }
    public override void AddAALayer()
    {
        _instanceConfig.Size = new Vector3(_instanceConfig.Size.x, _instanceConfig.Size.y, _instanceConfig.Size.z + 1);
        instanceCount = (int)(_instanceConfig.Size.x * _instanceConfig.Size.y * _instanceConfig.Size.z);
        _nativeMatrices = new NativeArray<Matrix4x4>(instanceCount, Allocator.Persistent);
    }

    public override void InstanceUpdate()
    {
        _job.Matrices = _nativeMatrices;
        _job.Time = Time.time;
        _job.size = _instanceConfig.Size;
        _job.Schedule(_nativeMatrices.Length, 64).Complete();
        Graphics.RenderMeshInstanced(rp, mesh, 0, _nativeMatrices);
    }
    public void OnDestroy()
    {
        if (instanceCount > 0)
            _nativeMatrices.Dispose();
    }
}
[BurstCompile]
internal struct GPUPositionJob : IJobParallelFor
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
