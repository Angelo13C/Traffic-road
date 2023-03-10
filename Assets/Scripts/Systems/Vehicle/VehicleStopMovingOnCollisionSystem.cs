using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[BurstCompile]
public partial struct VehicleStopMovingOnCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBufferSystem = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        var entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged);
        
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        state.Dependency = new RemoveComponentCollisionEvents 
        {
            CollisionWorld = physicsWorld.CollisionWorld,
            EntityCommandBuffer = entityCommandBuffer
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    //[BurstCompile]
    private struct RemoveComponentCollisionEvents : ICollisionEventsJob
    {
        [ReadOnly] public CollisionWorld CollisionWorld;
        public EntityCommandBuffer EntityCommandBuffer;

        private const byte DONT_STOP_VEHICLE_MOVER_CUSTOM_TAGS = 1 << 0;

        public void Execute(CollisionEvent collisionEvent)
        {
        if((collisionEvent.EntityA.Index != 103 && collisionEvent.EntityA.Index != 108) || (collisionEvent.EntityB.Index != 103 && collisionEvent.EntityB.Index != 108))
UnityEngine.Debug.Log("A: " + collisionEvent.EntityA + " | B: " + collisionEvent.EntityB);
            if((CollisionWorld.Bodies[collisionEvent.BodyIndexA].CustomTags & DONT_STOP_VEHICLE_MOVER_CUSTOM_TAGS) == 0 && 
               (CollisionWorld.Bodies[collisionEvent.BodyIndexB].CustomTags & DONT_STOP_VEHICLE_MOVER_CUSTOM_TAGS) == 0)
            {
                EntityCommandBuffer.RemoveComponent<VehicleMover>(collisionEvent.EntityA);
                EntityCommandBuffer.RemoveComponent<VehicleMover>(collisionEvent.EntityB);
            }
        }
    }
}