using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(UpdateCameraRaysSystem))]
public partial struct ThrowRockSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.TryGetSingleton<CameraRays>(out var cameraRays))
        {
            foreach (var (throwRock, transform) in SystemAPI.Query<RefRW<ThrowRockSP>, RefRW<LocalTransform>>().WithAll<SuperpowerTriggering>())
            {
                transform.ValueRW.Position = cameraRays.ScreenCenter.origin +
                                             cameraRays.ScreenCenter.direction *
                                             throwRock.ValueRO.DistanceFromEye;
            }

            foreach (var (throwRock, velocity, transform, entity) in SystemAPI.Query<ThrowRockSP, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>().WithAll<SuperpowerJustFinishedTriggering>().WithEntityAccess())
            {
                velocity.ValueRW.Linear = cameraRays.ScreenCenter.direction * throwRock.ThrowSpeed;
            }
        }
    }
}