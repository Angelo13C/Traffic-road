using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[BurstCompile]
public partial struct ChangeDragOnCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        state.Dependency = new ChangeDragJob 
        {
            CollisionWorld = physicsWorld.CollisionWorld,
            ChangeDragOnCollisionLookup = SystemAPI.GetComponentLookup<ChangeDragOnCollision>(true),
            PhysicsDampingLookup = SystemAPI.GetComponentLookup<PhysicsDamping>(false)
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    private struct ChangeDragJob : ICollisionEventsJob
    {
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public ComponentLookup<ChangeDragOnCollision> ChangeDragOnCollisionLookup;
        public ComponentLookup<PhysicsDamping> PhysicsDampingLookup;

        private const byte DONT_CHANGE_DRAG_TAGS = 1 << 0;

        public void Execute(CollisionEvent collisionEvent)
        {
            if((CollisionWorld.Bodies[collisionEvent.BodyIndexA].CustomTags & DONT_CHANGE_DRAG_TAGS) == 0 && 
               (CollisionWorld.Bodies[collisionEvent.BodyIndexB].CustomTags & DONT_CHANGE_DRAG_TAGS) == 0)
            {
                TryChangeDrag(collisionEvent.EntityA);
                TryChangeDrag(collisionEvent.EntityB);
            }
        }

        [BurstCompile]
        private void TryChangeDrag(Entity entity)
        {
            var dragEntity = PhysicsDampingLookup.GetRefRWOptional(entity, false);
            if(dragEntity.IsValid)
            {
                if(ChangeDragOnCollisionLookup.TryGetComponent(entity, out var changeDragOnCollision))
                    dragEntity.ValueRW.Linear = changeDragOnCollision.NewDrag;
            }
        }
    }
}