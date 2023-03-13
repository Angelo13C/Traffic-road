using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
public partial struct TimeFreezeSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var timeFreeze in SystemAPI.Query<RefRW<TimeFreezeSP>>())
        {
            timeFreeze.ValueRW.Duration -= deltaTime;
            var physicsStep = SystemAPI.GetSingletonRW<PhysicsStep>();
            if(timeFreeze.ValueRO.HasFinished)
                physicsStep.ValueRW.SimulationType = SimulationType.UnityPhysics;
            else
                physicsStep.ValueRW.SimulationType = SimulationType.NoPhysics;
        }
    }
}