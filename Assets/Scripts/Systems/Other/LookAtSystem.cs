using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct LookAtSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (lookAt, transform) in SystemAPI.Query<LookAt, RefRW<LocalTransform>>())
        {
            if(state.EntityManager.Exists(lookAt.EntityToLook))
            {
                var targetTransform = SystemAPI.GetComponent<LocalTransform>(lookAt.EntityToLook);
                transform.ValueRW.Rotation = lookAt.GetLookRotation(transform.ValueRO.Position, targetTransform.Position);
            }
        }
    }
}