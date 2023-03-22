using Unity.Entities;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public partial struct SuperpowerInputTriggerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (_, entity) in SystemAPI.Query<SuperpowerJustFinishedTriggering>().WithEntityAccess())
        {
            SystemAPI.SetComponentEnabled<SuperpowerJustFinishedTriggering>(entity, false);
        }
        foreach(var (superpowersInputTrigger, entity) in SystemAPI.Query<SuperpowerInputTrigger>().WithAll<SuperpowerTriggering>().WithNone<TeleportSP>().WithEntityAccess())
        {
            if (Input.GetKeyUp(superpowersInputTrigger.TriggerKey))
            {
                SystemAPI.SetComponentEnabled<SuperpowerTriggering>(entity, false);
                SystemAPI.SetComponentEnabled<SuperpowerJustFinishedTriggering>(entity, true);
            }
        }
    }
}