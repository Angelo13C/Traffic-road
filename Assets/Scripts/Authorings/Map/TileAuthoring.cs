using Unity.Entities;
using UnityEngine;

public class TileAuthoring : MonoBehaviour
{
	[SerializeField] private TileType _type;

	class Baker : Baker<TileAuthoring>
	{
		public override void Bake(TileAuthoring authoring)
		{
			if(authoring._type == TileType.Grass)
			{
				AddComponent(new GrassTile {
					JustSpawned = true
				});
			}
			else if(authoring._type == TileType.Road)
			{
				AddComponent(new RoadTile {
					JustSpawned = true,
					LastSpawnedDynamicObstacle = Entity.Null,
				});
			}
			else if(authoring._type == TileType.Water)
			{
				AddComponent(new WaterTile {
					JustSpawned = true,
					LastSpawnedDynamicObstacle = Entity.Null
				});
			}
		}
	}
}