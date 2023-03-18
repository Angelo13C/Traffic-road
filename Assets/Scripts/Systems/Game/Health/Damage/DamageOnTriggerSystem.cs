using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct DamageOnTriggerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new TriggerEventJob
        {
            PhysicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>(),
            DamageOnTrigger = SystemAPI.GetComponentLookup<DamageOnTrigger>(true),
            PhysicsVelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(true),
            Health = SystemAPI.GetComponentLookup<Health>(false),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }
    
    [BurstCompile]
    public partial struct TriggerEventJob : ITriggerEventsJob
    {
        [ReadOnly] public PhysicsWorldSingleton PhysicsWorldSingleton;
        
        [ReadOnly] public ComponentLookup<DamageOnTrigger> DamageOnTrigger;
        [ReadOnly] public ComponentLookup<PhysicsVelocity> PhysicsVelocityLookup;
        public ComponentLookup<Health> Health;

        public void Execute(TriggerEvent triggerEvent)
        {
            var damagerEntity = Entity.Null;
            var entityToDamage = Entity.Null;
            if(DamageOnTrigger.HasComponent(triggerEvent.EntityA))
            {
                damagerEntity = triggerEvent.EntityA;
                entityToDamage = triggerEvent.EntityB;
            }
            if(DamageOnTrigger.HasComponent(triggerEvent.EntityB))
            {
                damagerEntity = triggerEvent.EntityB;
                entityToDamage = triggerEvent.EntityA;
            }
            
            if(damagerEntity != Entity.Null)
            {
                var damageOnTrigger = DamageOnTrigger.GetRefRO(damagerEntity);
                var damageToDeal = damageOnTrigger.ValueRO.DamageToDealForMinVelocity;
                if(PhysicsVelocityLookup.TryGetComponent(damagerEntity, out var physicsVelocity))
                {
                    if(math.lengthsq(physicsVelocity.Linear) < damageOnTrigger.ValueRO.MinVelocityToDamageSqr)
                        damageToDeal = 0;
                }
                
                if(damageToDeal != 0)
                {
                    var health = Health.GetRefRWOptional(entityToDamage, false);
                    if(health.IsValid)
                        health.ValueRW.Current -= damageToDeal;
                }
            }
        }
    }
}