using Unity.Entities;
using UnityEngine;

public class MapAuthoring : MonoBehaviour
{
	[System.Serializable]
	private struct TilePrefab
	{
		public Tile Tile;
		public GameObject Prefab;
	}
	[SerializeField] private TilePrefab[] _tilePrefabs = new TilePrefab[1];

	class Baker : Baker<MapAuthoring>
	{
		public override void Bake(MapAuthoring authoring)
		{
			var map = new Map {

			};

			AddComponent(map);

			var mapTilePrefabs = AddBuffer<MapTilePrefab>();
			mapTilePrefabs.ResizeUninitialized(authoring._tilePrefabs.Length);
			for(var i = 0; i < authoring._tilePrefabs.Length; i++)
			{
				mapTilePrefabs[i] = new MapTilePrefab {
					Tile = authoring._tilePrefabs[i].Tile,
					Prefab = GetEntity(authoring._tilePrefabs[i].Prefab)
				};
			}
		}
	}
}