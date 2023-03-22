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
                entityCommandBuffer.DestroyEntity(entity);
        }
        foreach(var (jetpackSP, entity) in SystemAPI.Query<JetpackSP>().WithEntityAccess())
        {
            if(jetpackSP.HasFinished)
                entityCommandBuffer.DestroyEntity(entity);
        }
        foreach(var (timeFreezeSP, entity) in SystemAPI.Query<TimeFreezeSP>().WithEntityAccess())
        {
            if(timeFreezeSP.HasFinished)
                entityCommandBuffer.DestroyEntity(entity);
        }
        foreach(var (superSpeedSP, entity) in SystemAPI.Query<SuperSpeedSP>().WithEntityAccess())
        {
            if(superSpeedSP.HasFinished)
                entityCommandBuffer.DestroyEntity(entity);
        }
        entityCommandBuffer.Playback(state.EntityManager);
    }
}