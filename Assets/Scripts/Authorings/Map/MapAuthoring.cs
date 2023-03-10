using Unity.Entities;
using UnityEngine;
using Unity.Collections;

public class MapAuthoring : MonoBehaviour
{
	[SerializeField] [Min(1)] private int _seed = 1;

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
				Rng = new Unity.Mathematics.Random((uint) authoring._seed),
			};
			AddComponent(map);

			var mapTiles = AddBuffer<MapTile>();
			mapTiles.EnsureCapacity(500);

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
				
				var consecutiveTilesArraySize = 0;
				for(var i = 0; i < authoring._tileConfigs.Length; i++)
				{
					var tileConfig = authoring._tileConfigs[i];
					for(var j = 0; j < tileConfig.ConsecutiveTilesSpawnRateWeight.Length; j++)
						consecutiveTilesArraySize += tileConfig.ConsecutiveTilesSpawnRateWeight[j];
				}

				var consecutiveTiles = builder.Allocate(ref tilesBlob.ConsecutiveTilesSpawnRateWeight, consecutiveTilesArraySize);
				for(var i = 0; i < configsBlob.Length; i++)
				{
					var authoringTile = authoring._tileConfigs[i];
					var totalWeightsSum = 0;
					var lastIndex = i == 0 ? 0 : configsBlob[i - 1].ConsecutiveTilesSpawnRateWeightEndIndex;
					for(var j = 0; j < authoring._tileConfigs[i].ConsecutiveTilesSpawnRateWeight.Length; j++)
					{
						consecutiveTiles[lastIndex + j] = authoringTile.ConsecutiveTilesSpawnRateWeight[j];
						totalWeightsSum += consecutiveTiles[j];
					}

					ref var mapTileConfig = ref configsBlob[i];
					mapTileConfig.Tile = authoringTile.Tile;
					mapTileConfig.AllowedNextTiles = authoringTile.AllowedNextTiles;
					mapTileConfig.SpawnRateWeight = authoringTile.SpawnRateWeight;
					mapTileConfig.TotalConsecutiveTilesSpawnRateWeights = totalWeightsSum;
					mapTileConfig.ConsecutiveTilesSpawnRateWeightStartIndex = lastIndex;
					mapTileConfig.ConsecutiveTilesSpawnRateWeightEndIndex = lastIndex + authoring._tileConfigs[i].ConsecutiveTilesSpawnRateWeight.Length;
				}

				var blobAsset = builder.CreateBlobAssetReference<MapTileConfigsArray>(Allocator.Persistent);
			
				mapTileConfigs.BlobRefeence = blobAsset;
			}
			AddComponent(mapTileConfigs);
		}
	}
}