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
        foreach(var (doubleJumpSP, character, characterBody) in SystemAPI.Query<RefRW<DoubleJumpSP>, FirstPersonCharacterComponent, RefRW<KinematicCharacterBody>>())
        {
            if(characterBody.ValueRO.IsGrounded)
                doubleJumpSP.ValueRW.CurrentInAirJumpsCount = 0;

            if(Input.GetKeyDown(KeyCode.Space))
                doubleJumpSP.ValueRW.CurrentInAirJumpsCount++;

            if(doubleJumpSP.ValueRO.CurrentInAirJumpsCount == 2)
            {
                var jumpVelocity = math.up() * doubleJumpSP.ValueRO.Force;
                CharacterControlUtilities.StandardJump(ref characterBody.ValueRW, jumpVelocity, false, math.up());
                doubleJumpSP.ValueRW.CurrentInAirJumpsCount++;
            }
        }
    }
}