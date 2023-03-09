using Unity.Entities;
using Unity.Mathematics;

public struct VehicleMover : IComponentData
{
    public float Speed;

    public readonly static float3 MOVE_DIRECTION = new float3(-1, 0, 0);
}