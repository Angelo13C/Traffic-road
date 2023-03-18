using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct GrowOverTimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var (growOverTime, transform) in SystemAPI.Query<GrowOverTime, RefRW<LocalTransform>>())
        {
            transform.ValueRW.Scale += growOverTime.GrowthPerSecond * deltaTime;
        }
    }
}