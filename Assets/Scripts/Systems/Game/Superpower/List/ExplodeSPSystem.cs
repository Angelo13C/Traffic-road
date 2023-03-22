using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct ExplodeSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (transform, explosive, entity) in SystemAPI.Query<LocalTransform, Explosive>().WithAll<ExplodeSP>().WithEntityAccess())
        {
            explosive.Explode(entityCommandBuffer, transform.Position);
            entityCommandBuffer.DestroyEntity(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}