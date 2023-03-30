using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Player", menuName = "Scriptable Objects/Audio Player")]
public class AudioPlayer : ScriptableObject
{
    [field: SerializeField] public EventReference HornSound { get; private set; }
    [field: SerializeField] public EventReference CarEngineSound { get; private set; }

    public void PlaySingle(EventReference audio, float3 audioPosition)
    {
        RuntimeManager.PlayOneShot(audio, audioPosition);
    }
}