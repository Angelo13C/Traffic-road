using Unity.Entities;
using Unity.Mathematics;

public struct LookAt : IComponentData
{
    public Entity EntityToLook;

    public quaternion GetLookRotation(float3 myPosition, float3 entityToLookAtPosition)
    {
        var direction = math.normalize(myPosition - entityToLookAtPosition);
        return quaternion.LookRotation(direction, math.up());
    }
}