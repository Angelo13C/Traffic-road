using Unity.Entities;
using UnityEngine;
using Unity.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapAuthoring : MonoBehaviour
{
	[HideInInspector] public bool RandomSeed = false;
	[HideInInspector] public int Seed = 1;

	[System.Serializable]
	private struct TilePrefab
	{
		public TileType Tile;
		public GameObject Prefab;
	}
	[SerializeField] private TilePrefab[] _tilePrefabs = new TilePrefab[1];
	
	[System.Serializable]
	private struct TileConfig
	{
		public TileType Tile;
		public TileType AllowedNextTiles;

		public int SpawnRateWeight;
		public int[] ConsecutiveTilesSpawnRateWeight;
	}
	[SerializeField] private TileConfig[] _tileConfigs = new TileConfig[1];

	class Baker : Baker<MapAuthoring>
	{
		public override void Bake(MapAuthoring authoring)
		{
			var seed = (uint) (authoring.RandomSeed ? System.DateTime.Now.Ticks : authoring.Seed);
			if (seed == 0)
				seed = 1;
			var map = new Map {
				Rng = new Unity.Mathematics.Random(seed),
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

#if UNITY_EDITOR
[CustomEditor(typeof(MapAuthoring))]
public class MapAuthoringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapAuthoring = target as MapAuthoring;
        mapAuthoring.RandomSeed = EditorGUILayout.Toggle("Random seed", mapAuthoring.RandomSeed);

        if (!mapAuthoring.RandomSeed)
        {
            mapAuthoring.Seed = EditorGUILayout.IntField("Seed", mapAuthoring.Seed);
			mapAuthoring.Seed = Mathf.Max(mapAuthoring.Seed, 1);
        }

        base.OnInspectorGUI();
    }
}
#endif