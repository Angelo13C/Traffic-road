using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[CreateAssetMenu(fileName = "Audio Player", menuName = "Scriptable Objects/Audio Player")]
public class AudioPlayer : ScriptableObject
{
    [field: SerializeField] public EventReference HornSound { get; private set; }
    [field: SerializeField] public EventReference CarEngineSound { get; private set; }
    
    [field: Header("Player")]
    [field: SerializeField] public EventReference PlayerFootstep { get; private set; }
    [SerializeField] private string _playerFootstepSurfaceName = "Surface";
    public enum FootstepSurface
    {
        Grass,
        Road,
        None
    }
    private EventInstance _playerFootstepInstance;

    private List<EventInstance> _eventInstances = new List<EventInstance>(30);

    public void PlaySingle(EventReference audio, float3 audioPosition)
    {
        RuntimeManager.PlayOneShot(audio, audioPosition);
    }

    public EventInstance CreateInstance(EventReference audio)
    {
        var eventInstance = RuntimeManager.CreateInstance(audio);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void SetPlayerFootstepSurface(FootstepSurface surface, float3 position = new float3())
    {
        if (!_playerFootstepInstance.isValid())
            _playerFootstepInstance = CreateInstance(PlayerFootstep);
        
        _playerFootstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        if (surface == FootstepSurface.None)
            _playerFootstepInstance.stop(STOP_MODE.ALLOWFADEOUT);
        else
        {
            _playerFootstepInstance.setParameterByNameWithLabel(_playerFootstepSurfaceName, surface.ToString());
            
            _playerFootstepInstance.getPlaybackState(out var playbackState);
            if (playbackState == PLAYBACK_STATE.STOPPED)
                _playerFootstepInstance.start();
        }
    }
}