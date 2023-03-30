using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class TimeFreezeVolumeEffect : MonoBehaviour
{
	[Header("Fade time")]
	[SerializeField] [Range(0, 1)] private float _startFadeTime = 0.2f;
	[SerializeField] [Range(0, 1)] private float _endFadeTime = 0.2f;
	
	[Header("Pulse")]
	[SerializeField] [Range(0, 0.5f)] private float _pulseStrength = 0.1f;
	[SerializeField] [Range(0, 10)] private float _pulseSpeed = 3f;

	private Volume _timeFreezeVolume;
	private EntityQuery _timeFreezeSPQuery;

	private void Start()
	{
		_timeFreezeVolume = GetComponent<Volume>();
		_timeFreezeSPQuery =
			World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(TimeFreezeSP));
	}

	private void Update()
	{
		var isTimeFrozen = !_timeFreezeSPQuery.IsEmpty;
		if (isTimeFrozen)
		{
			if (_timeFreezeVolume.weight >= 1 - _pulseStrength)
			{
				var weightOffset = math.abs(_pulseStrength * math.sin(Time.time * _pulseSpeed));
				SetWeight(1 - weightOffset);
			}
			else
				SetWeight(_timeFreezeVolume.weight + Time.deltaTime / _startFadeTime);
		}
		else
			SetWeight(_timeFreezeVolume.weight - Time.deltaTime / _endFadeTime);
	}
	
	private void SetWeight(float value) => _timeFreezeVolume.weight = Mathf.Clamp(value, 0f, 1f);
}