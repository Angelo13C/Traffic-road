using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
public partial struct VehicleMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (vehicleMover, physicsVelocity) in SystemAPI.Query<VehicleMover, RefRW<PhysicsVelocity>>())
        {
            var velocity = vehicleMover.Speed * VehicleMover.MOVE_DIRECTION;
            physicsVelocity.ValueRW.Linear = velocity;
        }
    }
}