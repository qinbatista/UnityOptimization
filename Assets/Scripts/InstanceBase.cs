using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceBase : MonoBehaviour
{
    public abstract void Initial();
    public abstract void AddAALayer();
    public abstract void InstanceUpdate();
}
