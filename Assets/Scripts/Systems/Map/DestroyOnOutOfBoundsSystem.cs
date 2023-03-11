using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateBefore(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct DestroyOnOutOfBoundsSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBufferSystem = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        var entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged);
        foreach(var (transform, entity) in SystemAPI.Query<LocalTransform>().WithAll<DestroyOnOutOfBounds>().WithEntityAccess())
        {
            if(math.abs(transform.Position.x) > MapTilePrefab.TILE_LENGTH / 2)
                entityCommandBuffer.DestroyEntity(entity);
        }
    }
}