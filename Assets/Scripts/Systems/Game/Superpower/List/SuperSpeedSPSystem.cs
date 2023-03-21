using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateBefore(typeof(SuperpowerIsGoneSystem))]
public partial struct SuperSpeedSPSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var (superSpeedSP, character) in SystemAPI.Query<RefRW<SuperSpeedSP>, RefRW<FirstPersonCharacterComponent>>())
        {
            superSpeedSP.ValueRW.Duration -= deltaTime;
            if(!superSpeedSP.ValueRO.HasBeenApplied)
            {
                character.ValueRW.GroundMaxSpeed *= superSpeedSP.ValueRO.SpeedMultiplier;
                superSpeedSP.ValueRW.HasBeenApplied = true;
            }
            if(superSpeedSP.ValueRO.HasFinished)
            {
                character.ValueRW.GroundMaxSpeed /= superSpeedSP.ValueRO.SpeedMultiplier;
            }
        }
    }
}