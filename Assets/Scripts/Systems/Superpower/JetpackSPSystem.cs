using Unity.Burst;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct JetpackSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var (jetpackSP, transform, characterBody) in SystemAPI.Query<RefRW<JetpackSP>, LocalTransform, RefRW<KinematicCharacterBody>>())
        {
            if(transform.Position.y <= jetpackSP.ValueRO.MaxHeight && Input.GetKey(KeyCode.Space))
            {
                var jetpackVelocity = math.up() * jetpackSP.ValueRO.Force;
                CharacterControlUtilities.StandardJump(ref characterBody.ValueRW, jetpackVelocity, false, math.up());
                jetpackSP.ValueRW.Duration -= deltaTime;
            }
        }
    }
}