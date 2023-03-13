using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct SuperpowerIsGoneSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (doubleJumpSP, entity) in SystemAPI.Query<RefRW<DoubleJumpSP>>().WithEntityAccess())
        {
            doubleJumpSP.ValueRW.Duration -= deltaTime;
            if(doubleJumpSP.ValueRO.HasFinished)
                entityCommandBuffer.RemoveComponent<DoubleJumpSP>(entity);
        }
        entityCommandBuffer.Playback(state.EntityManager);
    }
}