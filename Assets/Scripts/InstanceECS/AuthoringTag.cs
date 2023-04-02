using Unity.Entities;
using UnityEngine;
public class AuthoringTag : MonoBehaviour
{
    public int _id;
}

public class InstanceECSTagBake : Baker<AuthoringTag>
{
    public override void Bake(AuthoringTag authoring)
    {
        Entity entity = this.GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
        AddComponent(entity, new AuthoringTagComponent()
        {
            _id = authoring._id
        });
    }
}
public struct AuthoringTagComponent : IComponentData
{
    public int _id;
}