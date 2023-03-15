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
                var direction = math.normalize(transform.ValueRO.Position - targetTransform.Position);
                transform.ValueRW.Rotation = quaternion.LookRotation(direction, math.up());
            }
        }
    }
}