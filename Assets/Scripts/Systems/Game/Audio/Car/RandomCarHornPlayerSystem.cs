using Unity.Entities;
using UnityEngine;

public partial class RandomCarHornPlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var nearbyCars in SystemAPI.Query<DynamicBuffer<NearbyCars>>())
        {
            if (!nearbyCars.IsEmpty)
            {
                foreach (var randomCarHornPlayer in SystemAPI.Query<RandomCarHornPlayer>())
                {
                    if (randomCarHornPlayer.AudioPlayer != null)
                    {
                        randomCarHornPlayer.RemainingTimeToHorn -= deltaTime;
                        if (randomCarHornPlayer.RemainingTimeToHorn <= 0)
                        {
                            randomCarHornPlayer.RemainingTimeToHorn = Random.Range(
                                randomCarHornPlayer.TimeRangeBetweenHorns.x,
                                randomCarHornPlayer.TimeRangeBetweenHorns.y);
                            
                            randomCarHornPlayer.AudioPlayer.PlaySingle(randomCarHornPlayer.AudioPlayer.HornSound, nearbyCars[0].Position);
                        }
                    }
                }
            }
        }
    }
}