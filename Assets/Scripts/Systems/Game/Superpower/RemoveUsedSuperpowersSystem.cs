using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct RemoveUsedSuperpowersSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (superpowerUser, heldSuperpowers) in SystemAPI.Query<RefRW<SuperpowerUser>, DynamicBuffer<HeldSuperpower>>())
        {
            if (superpowerUser.ValueRO.LastUsedSuperpower != Entity.Null && heldSuperpowers.Length > 0)
            {
                if (!state.EntityManager.Exists(superpowerUser.ValueRO.LastUsedSuperpower) || (SystemAPI.HasComponent<SuperpowerTriggering>(superpowerUser.ValueRO.LastUsedSuperpower) && !SystemAPI.IsComponentEnabled<SuperpowerTriggering>(superpowerUser.ValueRO.LastUsedSuperpower)))
                {
                    superpowerUser.ValueRW.LastUsedSuperpower = Entity.Null;
                    heldSuperpowers.RemoveAt(0);
                }
            }
        }
    }
}