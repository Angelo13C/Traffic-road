using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct UseSuperpowerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var inputTriggerLookup = SystemAPI.GetComponentLookup<SuperpowerInputTrigger>();
        foreach (var (superpowerUser, heldSuperpowers, entity) in SystemAPI.Query<RefRW<SuperpowerUser>, DynamicBuffer<HeldSuperpower>>().WithEntityAccess())
        {
            if(superpowerUser.ValueRO.LastUsedSuperpower == Entity.Null && heldSuperpowers.Length > 0)
            {
                var currentSuperpower = heldSuperpowers[0].Prefab;
                var inputTrigger = inputTriggerLookup.GetRefRO(currentSuperpower);
                var shouldUse = false;
                if(inputTrigger.ValueRO.TriggeringKey != KeyCode.None)
                {
                    if (Input.GetKeyDown(inputTrigger.ValueRO.TriggeringKey))
                        shouldUse = true;
                }
                else if (Input.GetKeyDown(inputTrigger.ValueRO.TriggerKey))
                    shouldUse = true;

                if (shouldUse)
                {
                    superpowerUser.ValueRW.LastUsedSuperpower = state.EntityManager.Instantiate(currentSuperpower);
                    SystemAPI.SetComponent(superpowerUser.ValueRW.LastUsedSuperpower, new TriggeredBy
                    {
                        By = entity
                    });
                }
            }
        }
    }
}