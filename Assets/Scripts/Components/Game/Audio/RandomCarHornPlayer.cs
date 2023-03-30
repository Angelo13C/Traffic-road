using Unity.Entities;
using Unity.Mathematics;

public class RandomCarHornPlayer : IComponentData
{
    public float2 TimeRangeBetweenHorns;
    public float RemainingTimeToHorn;
    public AudioPlayer AudioPlayer;
}