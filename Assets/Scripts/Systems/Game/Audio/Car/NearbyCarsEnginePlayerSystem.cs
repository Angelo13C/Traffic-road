using System.Numerics;
using FMOD;
using FMODUnity;
using Unity.Collections;
using Unity.Entities;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
                    for (var i = nearbyCarsEnginePlayer.CurrentlyPlayingSounds.Count; i < nearbyCars.Length; i++)
                    {
                        var newSound = nearbyCarsEnginePlayer.AudioPlayer.CreateInstance(nearbyCarsEnginePlayer.AudioPlayer.CarEngineSound);
                        nearbyCarsEnginePlayer.CurrentlyPlayingSounds.Add(newSound);
                        newSound.start();
                    }
                    for (var i = nearbyCars.Length; i < nearbyCarsEnginePlayer.CurrentlyPlayingSounds.Count; i++)
                    {
                        nearbyCarsEnginePlayer.CurrentlyPlayingSounds[i - 1].stop(STOP_MODE.ALLOWFADEOUT);
                        nearbyCarsEnginePlayer.CurrentlyPlayingSounds.RemoveAtSwapBack(i - 1);
                    }

                    for (var i = 0; i < nearbyCars.Length; i++)
                    {
                        var carAttributes = RuntimeUtils.To3DAttributes(nearbyCars[i].Position);
                        nearbyCarsEnginePlayer.CurrentlyPlayingSounds[i].set3DAttributes(carAttributes);
                    }
                }
            }
        }
    }
}