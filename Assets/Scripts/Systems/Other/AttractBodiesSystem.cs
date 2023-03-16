using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
[BurstCompile]
public partial struct AttractBodiesSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var invDeltaTime = 1f / SystemAPI.Time.DeltaTime;
        foreach(var (attractBodies, transform) in SystemAPI.Query<AttractBodies, LocalTransform>())
        {
            var nearbyBodies = new NativeList<DistanceHit>(10, Allocator.Temp);
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            var collisionFilter = new CollisionFilter {
                GroupIndex = CollisionFilter.Default.GroupIndex,
                BelongsTo = CollisionFilter.Default.BelongsTo,
                CollidesWith = attractBodies.AttractableBodiesTags.Value
            };
            if(physicsWorld.OverlapSphere(transform.Position, attractBodies.Range, ref nearbyBodies, collisionFilter))
            {
                var velocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(false);
                var physicsDampingLookup = SystemAPI.GetComponentLookup<PhysicsDamping>(false);
                var changeDragOnBlackHoleLookup = SystemAPI.GetComponentLookup<ChangeDragOnBlackHole>(true);
                for(var i = 0; i < nearbyBodies.Length; i++)
                {
                    var nearbyBodyVelocity = velocityLookup.GetRefRWOptional(nearbyBodies[i].Entity, false);
                    if(nearbyBodyVelocity.IsValid)
                    {
                        var direction = nearbyBodies[i].Position - transform.Position;
                        var intensity = math.length(direction) / attractBodies.Range;

                        var targetTransform = new RigidTransform(quaternion.identity, transform.Position);
                        physicsWorld.CalculateVelocityToTarget(nearbyBodies[i].RigidBodyIndex, targetTransform, attractBodies.Force * invDeltaTime, out var linearVelocity, out var _);
                        nearbyBodyVelocity.ValueRW.Linear = linearVelocity;

                        var dragOfSuckedEntity = physicsDampingLookup.GetRefRWOptional(nearbyBodies[i].Entity, false);
                        if(dragOfSuckedEntity.IsValid)
                        {
                            if(changeDragOnBlackHoleLookup.TryGetComponent(nearbyBodies[i].Entity, out var changeDragOnBlackHole))
                                dragOfSuckedEntity.ValueRW.Linear = changeDragOnBlackHole.NewDrag;
                        }
                    }
                }
            }
        }
    }
}