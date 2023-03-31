using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct NearbyCarsTracker : IComponentData
{
    public float Radius;
    public CollisionFilter CollisionFilter;
    public int MaxTrackedCarsCount;
}

[InternalBufferCapacity(5)]
public struct NearbyCars : IBufferElementData
{
    public Entity Entity;
    public float3 Position;
}