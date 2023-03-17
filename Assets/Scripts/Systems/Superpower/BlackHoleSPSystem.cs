using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BlackHoleSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (blackHole, superpowers, entity) in SystemAPI.Query<BlackHoleSP, Superpowers>().WithEntityAccess())
        {
            if(Input.GetMouseButtonUp(0))
            {
                var spawnedBlackHole = state.EntityManager.Instantiate(blackHole.BlackHolePrefab);

                if(SystemAPI.TryGetSingletonEntity<FirstPersonCharacterComponent>(out var playerEntity))
                {
                    var spawnedBlackHoleDistortionEntity = state.EntityManager.GetBuffer<LinkedEntityGroup>(spawnedBlackHole)[1].Value;
                    var lookAt = new LookAt {
                        EntityToLook = playerEntity
                    };
                    SystemAPI.SetComponent(spawnedBlackHoleDistortionEntity, lookAt);
                }

                SystemAPI.SetComponent(spawnedBlackHole, new LocalTransform {
                    Position = superpowers.Ray.origin + superpowers.Ray.direction * blackHole.SpawnDistanceFromEye,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });
                SystemAPI.SetComponent(spawnedBlackHole, new PhysicsVelocity {
                    Angular = float3.zero,
                    Linear = superpowers.Ray.direction * blackHole.ThrowSpeed
                });

                entityCommandBuffer.RemoveComponent<BlackHoleSP>(entity);
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}