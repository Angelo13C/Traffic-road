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
        foreach(var (jetpackSP, triggeredBy) in SystemAPI.Query<RefRW<JetpackSP>, TriggeredBy>())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                var transform = SystemAPI.GetComponent<LocalTransform>(triggeredBy.By);
                if(transform.Position.y <= jetpackSP.ValueRO.MaxHeight)
                {
                    var characterBody = SystemAPI.GetComponent<KinematicCharacterBody>(triggeredBy.By);
                    var jetpackVelocity = math.up() * jetpackSP.ValueRO.Force;
                    CharacterControlUtilities.StandardJump(ref characterBody, jetpackVelocity, false, math.up());
                    SystemAPI.SetComponent(triggeredBy.By, characterBody);
                    jetpackSP.ValueRW.Duration -= deltaTime;
                }
            }
        }
    }
}