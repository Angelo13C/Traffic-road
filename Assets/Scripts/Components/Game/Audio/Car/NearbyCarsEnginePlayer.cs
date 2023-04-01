using System.Collections.Generic;
using FMOD.Studio;
using Unity.Entities;

public class NearbyCarsEnginePlayer : IComponentData
{
    public AudioPlayer AudioPlayer;
    public List<EventInstance> CurrentlyPlayingSounds = new List<EventInstance>(5);
}