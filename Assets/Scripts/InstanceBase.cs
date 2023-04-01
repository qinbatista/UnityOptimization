using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceBase : MonoBehaviour
{
    public abstract void Initial(Vector3 size, GameObject gameObject);
    public abstract Vector3 AddAALayer();
    public abstract Vector3 ReduceALayer(bool destroyAll = false);
    public abstract void InstanceUpdate();
}
