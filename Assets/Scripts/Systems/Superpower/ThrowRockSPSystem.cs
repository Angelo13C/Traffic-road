using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(SuperpowersInputSystem))]
public partial struct ThrowRockSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (throwRock, superpowers, entity) in SystemAPI.Query<RefRW<ThrowRockSP>, Superpowers>().WithEntityAccess())
        {
            if(Input.GetMouseButtonDown(0))
            {
                throwRock.ValueRW.CurrentlyThrowingRock = state.EntityManager.Instantiate(throwRock.ValueRO.RockPrefab);
            }
            if(throwRock.ValueRW.CurrentlyThrowingRock != Entity.Null)
            {
                var throwingRockTransform = transformLookup.GetRefRW(throwRock.ValueRW.CurrentlyThrowingRock, false);
                throwingRockTransform.ValueRW.Position = superpowers.Ray.origin + superpowers.Ray.direction * throwRock.ValueRO.DistanceFromEye;

                if(Input.GetMouseButtonUp(0))
                {
                    SystemAPI.SetComponent(throwRock.ValueRO.CurrentlyThrowingRock, new PhysicsVelocity {
                        Angular = float3.zero,
                        Linear = superpowers.Ray.direction * throwRock.ValueRO.ThrowSpeed
                    });

                    entityCommandBuffer.RemoveComponent<ThrowRockSP>(entity);
                }
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}