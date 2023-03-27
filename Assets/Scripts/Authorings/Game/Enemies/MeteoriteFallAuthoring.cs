using Unity.Entities;
using UnityEngine;

public class MeteoriteFallAuthoring : MonoBehaviour
{
	[SerializeField] private GameObject _meteoritePrefab;
	[SerializeField] private float _startZ = -5f;
	[SerializeField] private float _zChangePerSecond = 0.5f;
	
	[SerializeField] private float _spawnHeight = 100f;
	
	[SerializeField] [Min(0)] private float _timeBetweenBehindMeteorites;
	[SerializeField] [Min(0)] private float _timeBetweenAheadMeteorites;

	class Baker : Baker<MeteoriteFallAuthoring>
	{
		public override void Bake(MeteoriteFallAuthoring authoring)
		{
			var meteoriteFall = new MeteoriteFall
			{
				MeteoritePrefab = GetEntity(authoring._meteoritePrefab),
				CurrentZ = authoring._startZ,
				ZChangePerSecond = authoring._zChangePerSecond,
				SpawnHeight = authoring._spawnHeight,
				BehindConfig = new MeteoriteFall.Config
				{
					RemainingTime = authoring._timeBetweenBehindMeteorites,
					TimeBetweenMeteorites = authoring._timeBetweenBehindMeteorites
				},
				AheadConfig = new MeteoriteFall.Config
				{
					RemainingTime = authoring._timeBetweenAheadMeteorites,
					TimeBetweenMeteorites = authoring._timeBetweenAheadMeteorites
				}
			};
			
			AddComponent(meteoriteFall);
		}
	}
}