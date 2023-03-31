using Unity.Entities;

public class FootstepAudioPlayer : IComponentData
{
    public AudioPlayer AudioPlayer;
    public float FootYOffset;
}