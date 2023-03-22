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
        foreach(var (superSpeedSP, triggeredBy) in SystemAPI.Query<RefRW<SuperSpeedSP>, TriggeredBy>())
        {
            superSpeedSP.ValueRW.Duration -= deltaTime;
            if(!superSpeedSP.ValueRO.HasBeenApplied)
            {
                var character = SystemAPI.GetComponent<FirstPersonCharacterComponent>(triggeredBy.By);
                character.GroundMaxSpeed *= superSpeedSP.ValueRO.SpeedMultiplier;
                SystemAPI.SetComponent(triggeredBy.By, character);
                superSpeedSP.ValueRW.HasBeenApplied = true;
            }
            if(superSpeedSP.ValueRO.HasFinished)
            {
                var character = SystemAPI.GetComponent<FirstPersonCharacterComponent>(triggeredBy.By);
                character.GroundMaxSpeed /= superSpeedSP.ValueRO.SpeedMultiplier;
                SystemAPI.SetComponent(triggeredBy.By, character);
            }
        }
    }
}