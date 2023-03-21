using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(SuperpowerTriggerResetSystem))]
public partial struct SuperpowerInputTriggerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var superpowerTriggeringLookup = SystemAPI.GetComponentLookup<SuperpowerTriggering>();
        var superpowerTriggeredLookup = SystemAPI.GetComponentLookup<SuperpowerTriggered>();
        foreach(var (superpowersInputTrigger, entity) in SystemAPI.Query<SuperpowerInputTrigger>().WithDisabled<SuperpowerTriggering>().WithEntityAccess())
        {
            if(Input.GetKeyDown(superpowersInputTrigger.TriggerKey))
                superpowerTriggeringLookup.SetComponentEnabled(entity, true);
        }
        foreach(var (superpowersInputTrigger, entity) in SystemAPI.Query<SuperpowerInputTrigger>().WithDisabled<SuperpowerTriggered>().WithEntityAccess())
        {
            if (superpowerTriggeringLookup.HasComponent(entity))
            {
                if(Input.GetKeyUp(superpowersInputTrigger.TriggerKey))
                    superpowerTriggeredLookup.SetComponentEnabled(entity, true);
            }
            else
            {
                if(Input.GetKeyDown(superpowersInputTrigger.TriggerKey))
                    superpowerTriggeredLookup.SetComponentEnabled(entity, true);
            }
        }
    }
}