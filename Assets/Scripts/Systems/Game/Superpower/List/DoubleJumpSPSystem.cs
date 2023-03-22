using Unity.Burst;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public partial struct DoubleJumpSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (doubleJump, triggeredBy) in SystemAPI.Query<RefRW<DoubleJumpSP>, TriggeredBy>())
        {
            var characterBody = SystemAPI.GetComponent<KinematicCharacterBody>(triggeredBy.By);
            if(characterBody.IsGrounded)
                doubleJump.ValueRW.CurrentInAirJumpsCount = 0;

            if(Input.GetKeyDown(KeyCode.Space))
                doubleJump.ValueRW.CurrentInAirJumpsCount++;

            if(doubleJump.ValueRO.CurrentInAirJumpsCount == 2)
            {
                var jumpVelocity = math.up() * doubleJump.ValueRO.Force;
                CharacterControlUtilities.StandardJump(ref characterBody, jumpVelocity, false, math.up());
                SystemAPI.SetComponent(triggeredBy.By, characterBody);
                doubleJump.ValueRW.CurrentInAirJumpsCount++;
            }
        }
    }
}