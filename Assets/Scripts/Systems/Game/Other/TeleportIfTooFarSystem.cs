using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct TeleportIfTooFarSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (teleportIfTooFar, transform) in SystemAPI.Query<TeleportIfTooFar, RefRW<LocalTransform>>())
        {
            if(teleportIfTooFar.From != Entity.Null)
            {
                if(state.EntityManager.Exists(teleportIfTooFar.From))
                {
                    var fromTransform = SystemAPI.GetComponent<LocalTransform>(teleportIfTooFar.From);
                    var zDistance = fromTransform.Position.z - transform.ValueRO.Position.z;
                    if(math.abs(zDistance) >= teleportIfTooFar.MaxZDistance)
                    {
                        transform.ValueRW.Position.z = fromTransform.Position.z;
                    }
                }
            }
        }
    }
}