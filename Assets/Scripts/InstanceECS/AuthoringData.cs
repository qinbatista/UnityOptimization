using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
public class AuthoringData : MonoBehaviour
{
    public Vector3 position;
    public double speed;
}

public class DataBaker : Baker<AuthoringData>
{
    public override void Bake(AuthoringData authoring)
    {
        Entity _entity = this.GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
        AddComponent(_entity, new AuthoringDataComponent
        {
            position = authoring.position,
            speed = authoring.speed,
        });
    }
}
public struct AuthoringDataComponent : IComponentData
{
    public Vector3 position;
    public double speed;
}

