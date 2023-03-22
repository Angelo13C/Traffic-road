using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(UpdateCameraRaysSystem))]
public partial struct BlackHoleSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingleton<CameraRays>(out var cameraRays))
        {
            foreach(var (blackHole, transform, triggeredBy) in SystemAPI.Query<BlackHoleSP, RefRW<LocalTransform>, TriggeredBy>().WithAll<SuperpowerTriggering>())
            {
                transform.ValueRW.Position = cameraRays.ScreenCenter.origin +
                                             cameraRays.ScreenCenter.direction * blackHole.SpawnDistanceFromEye;
            }
            
            foreach(var (blackHole, transform, entity) in SystemAPI.Query<BlackHoleSP, RefRW<LocalTransform>>().WithAll<SuperpowerJustFinishedTriggering>().WithEntityAccess())
            {
                if(SystemAPI.TryGetSingletonEntity<FirstPersonCharacterComponent>(out var playerEntity))
                {
                    var spawnedBlackHoleDistortionEntity = state.EntityManager.GetBuffer<LinkedEntityGroup>(entity)[1].Value;
                    var lookAt = new LookAt {
                        EntityToLook = playerEntity
                    };
                    SystemAPI.SetComponent(spawnedBlackHoleDistortionEntity, lookAt);

                    SystemAPI.SetComponent(entity, new PhysicsVelocity
                    {
                        Linear = cameraRays.ScreenCenter.direction * blackHole.ThrowSpeed 
                    });
                    SystemAPI.SetComponent(entity, new PhysicsGravityFactor
                    {
                        Value = 1
                    });
                }
            }
        }
    }
}