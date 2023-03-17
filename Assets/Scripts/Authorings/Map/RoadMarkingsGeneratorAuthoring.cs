using Unity.Entities;
using UnityEngine;

public class RoadMarkingsGeneratorAuthoring : MonoBehaviour
{
	[SerializeField] private GameObject _roadMarkingsPrefab;

	class Baker : Baker<RoadMarkingsGeneratorAuthoring>
	{
		public override void Bake(RoadMarkingsGeneratorAuthoring authoring)
		{
			var roadMarkingsGenerator = new RoadMarkingsGenerator {
				RoadMarkingsPrefab = GetEntity(authoring._roadMarkingsPrefab),
				LastSpawnedTileIndex = 0
			};

			AddComponent(roadMarkingsGenerator);
		}
	}
}