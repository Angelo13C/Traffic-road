using Unity.Entities;
using UnityEngine;
using Unity.Collections;

public class MapAuthoring : MonoBehaviour
{
	[System.Serializable]
	private struct TilePrefab
	{
		public Tile Tile;
		public GameObject Prefab;
	}
	[SerializeField] private TilePrefab[] _tilePrefabs = new TilePrefab[1];
	
	[System.Serializable]
	private struct TileConfig
	{
		public Tile Tile;
		public Tile AllowedNextTiles;

		public int SpawnRateWeight;
		public int[] ConsecutiveTilesSpawnRateWeight;
	}
	[SerializeField] private TileConfig[] _tileConfigs = new TileConfig[1];

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

			var mapTileConfigs = new MapTileConfigs();
			using(var builder = new BlobBuilder(Allocator.Temp))
			{
				ref var tilesBlob = ref builder.ConstructRoot<MapTileConfigsArray>();
				
				var configsBlob = builder.Allocate(ref tilesBlob.Configs, authoring._tileConfigs.Length);
				for(var i = 0; i < configsBlob.Length; i++)
				{
					var authoringTile = authoring._tileConfigs[i];
					var consecutiveTiles = builder.Allocate(ref configsBlob[i].ConsecutiveTilesSpawnRateWeight, authoringTile.ConsecutiveTilesSpawnRateWeight.Length);
					var totalWeightsSum = 0;
					for(var j = 0; j < consecutiveTiles.Length; j++)
					{
						consecutiveTiles[j] = authoringTile.ConsecutiveTilesSpawnRateWeight[j];
						totalWeightsSum += consecutiveTiles[j];
					}

					ref var mapTileConfig = ref configsBlob[i];
					mapTileConfig.Tile = authoringTile.Tile;
					mapTileConfig.AllowedNextTiles = authoringTile.AllowedNextTiles;
					mapTileConfig.SpawnRateWeight = authoringTile.SpawnRateWeight;
					mapTileConfig._totalConsecutiveTilesSpawnRateWeights = totalWeightsSum;
				}

				var blobAsset = builder.CreateBlobAssetReference<MapTileConfigsArray>(Allocator.Persistent);
			
				mapTileConfigs.BlobRefeence = blobAsset;
			}
			AddComponent(mapTileConfigs);
		}
	}
}