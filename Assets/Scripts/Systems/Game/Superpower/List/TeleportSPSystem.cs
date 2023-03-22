using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(UpdateCameraRaysSystem))]
public partial struct TeleportSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingleton<CameraRays>(out var cameraRays))
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach(var (teleport, triggeredBy, entity) in SystemAPI.Query<TeleportSP, TriggeredBy>().WithEntityAccess())
            {
                var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
                var ray = new RaycastInput {
                    Start = cameraRays.Mouse.origin,
                    End = cameraRays.Mouse.origin + cameraRays.Mouse.direction * teleport.MaxDistance,
                    Filter = new CollisionFilter {
                        BelongsTo = CollisionFilter.Default.BelongsTo,
                        CollidesWith = teleport.HittableFilter.Value,
                        GroupIndex = CollisionFilter.Default.GroupIndex
                    }
                };
                if(physicsWorld.CastRay(ray, out var hit))
                {
                    var transform = SystemAPI.GetComponent<LocalTransform>(triggeredBy.By);
                    transform.Position = hit.Position;
                    SystemAPI.SetComponent(triggeredBy.By, transform);
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}