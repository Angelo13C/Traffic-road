using Unity.Entities;

public partial class NearbyCarsEnginePlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var nearbyCars in SystemAPI.Query<DynamicBuffer<NearbyCars>>())
        {
            if (!nearbyCars.IsEmpty)
            {
                foreach (var nearbyCarsEnginePlayer in SystemAPI.Query<NearbyCarsEnginePlayer>())
                {
                    foreach(var nearbyCar in nearbyCars)
                        nearbyCarsEnginePlayer.AudioPlayer.PlaySingle(nearbyCarsEnginePlayer.AudioPlayer.CarEngineSound, nearbyCar.Position);
                }
            }
        }
    }
}