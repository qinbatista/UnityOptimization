using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public partial class DynamicObjectPool : MonoBehaviour
{
    Mesh mesh;
    Material material;
    int instanceCount;
    Matrix4x4[] matrices;
    RenderParams rp;
    private ComputeBuffer instanceTransformsBuffer;
    private ComputeBuffer indirectArgsBuffer;
    public void GPUInstanceAddALayerObjects()
    {
        mesh = _gameObject.GetComponent<MeshFilter>().sharedMesh;
        material = _gameObject.GetComponent<MeshRenderer>().sharedMaterial;
        rp = new RenderParams(material);
        instanceCount = (int)(_size.x * _size.y * _size.z);
        print(instanceCount);
        matrices = new Matrix4x4[instanceCount];

    }
    public void GPUInstanceReduceALayerObjects(bool destroyAll = false)
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
    public void GPUInstanceUpdate()
    {
        int _objIndex = 0;
        for (int z = 0; z < _size.z; z++)
            for (int y = 0; y < _size.y; y++)
                for (int x = 0; x < _size.x; x++)
                {
                    Vector3 position = new Vector3(x, y, z * 5 + Mathf.Sin(Time.time + _objIndex));
                    matrices[y * (int)_size.y + x].SetTRS(position, Quaternion.identity, Vector3.one);
                    if (_objIndex % 1000 == 0)
                    {
                        // print("_objIndex=" + _objIndex + "  _objIndex % 1000=" + _objIndex % 1000);
                        Graphics.RenderMeshInstanced(rp, mesh, 0, matrices);
                    }
                    _objIndex++;
                }
        // print("one update");
    }
}
