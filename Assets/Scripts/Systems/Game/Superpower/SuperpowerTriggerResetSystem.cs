using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct SuperpowerTriggerResetSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (_, entity) in SystemAPI.Query<SuperpowerTriggering>().WithAll<SuperpowerTriggered>().WithEntityAccess())
        {
            SystemAPI.SetComponentEnabled<SuperpowerTriggering>(entity, false);
        }
        foreach(var (_, entity) in SystemAPI.Query<SuperpowerTriggered>().WithEntityAccess())
        {
            SystemAPI.SetComponentEnabled<SuperpowerTriggered>(entity, false);
        }
    }
}