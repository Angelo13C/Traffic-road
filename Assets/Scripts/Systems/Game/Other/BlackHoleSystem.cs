using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
public partial struct BlackHoleSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (blackHole, attractBodies, transform) in SystemAPI.Query<RefRW<BlackHole>, RefRW<AttractBodies>, LocalTransform>().WithNone<PhysicsVelocity>())
        {
            var deltaScale = transform.Scale - blackHole.ValueRO.LastScale;
            attractBodies.ValueRW.Range += deltaScale;
            blackHole.ValueRW.LastScale = transform.Scale;
        }
    }
}

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
[BurstCompile]
public partial struct BlackHoleBecomeStaticSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var (blackHole, transform, entity) in SystemAPI.Query<RefRW<BlackHole>, LocalTransform>().WithAll<PhysicsVelocity>().WithNone<SuperpowerTriggering>().WithEntityAccess())
        {
            blackHole.ValueRW.BecomeStaticRadius += blackHole.ValueRO.BecomeStaticRadiusGrowthPerSecond * deltaTime;
            
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            var collisionFilter = new CollisionFilter {
                GroupIndex = CollisionFilter.Default.GroupIndex,
                BelongsTo = CollisionFilter.Default.BelongsTo,
                CollidesWith = blackHole.ValueRO.BecomeStaticOnTouchTags.Value
            };
            var hit = new AnyHitCollector<DistanceHit>(blackHole.ValueRO.BecomeStaticRadius);
            if(physicsWorld.OverlapSphereCustom(transform.Position, blackHole.ValueRO.BecomeStaticRadius, ref hit, collisionFilter))
            {
                entityCommandBuffer.RemoveComponent<PhysicsVelocity>(entity);
                entityCommandBuffer.RemoveComponent<PhysicsDamping>(entity);
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}