using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(SuperpowersInputSystem))]
public partial struct TeleportSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (teleport, transform, superpowers, entity) in SystemAPI.Query<TeleportSP, RefRW<LocalTransform>, Superpowers>().WithEntityAccess())
        {
            if(Input.GetMouseButtonDown(0))
            {
                var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
                var ray = new RaycastInput {
                    Start = superpowers.Ray.origin,
                    End = superpowers.Ray.origin + superpowers.Ray.direction * teleport.MaxDistance,
                    Filter = new CollisionFilter {
                        BelongsTo = CollisionFilter.Default.BelongsTo,
                        CollidesWith = teleport.HittableFilter.Value,
                        GroupIndex = CollisionFilter.Default.GroupIndex
                    }
                };
                if(physicsWorld.CastRay(ray, out var hit))
                {
                    transform.ValueRW.Position = hit.Position;
                    entityCommandBuffer.RemoveComponent<TeleportSP>(entity);
                }
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}