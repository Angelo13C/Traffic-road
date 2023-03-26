using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class TimeFreezeVolumeEffect : MonoBehaviour
{
	[SerializeField] [Range(0, 1)] private float _fadeTime = 0.2f;

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
		var deltaWeight = Time.deltaTime / _fadeTime;
		if(!isTimeFrozen)
			deltaWeight *= -1;
		_timeFreezeVolume.weight = Mathf.Clamp(_timeFreezeVolume.weight + deltaWeight, 0f, 1f);
	}
}