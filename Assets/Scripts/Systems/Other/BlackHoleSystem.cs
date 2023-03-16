using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct BlackHoleSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (blackHole, attractBodies, transform) in SystemAPI.Query<RefRW<BlackHole>, RefRW<AttractBodies>, LocalTransform>())
        {
            var deltaScale = transform.Scale - blackHole.ValueRO.LastScale;
            attractBodies.ValueRW.Range += deltaScale;
            blackHole.ValueRW.LastScale = transform.Scale;
        }
    }
}