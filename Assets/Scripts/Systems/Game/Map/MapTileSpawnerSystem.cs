using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct MapTileSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var grassTile in SystemAPI.Query<RefRW<GrassTile>>())
            grassTile.ValueRW.JustSpawned = false;
        foreach(var roadTile in SystemAPI.Query<RefRW<RoadTile>>())
            roadTile.ValueRW.JustSpawned = false;
        foreach(var waterTile in SystemAPI.Query<RefRW<WaterTile>>())
            waterTile.ValueRW.JustSpawned = false;

        var rng = new Random(1 + (uint) ((SystemAPI.Time.ElapsedTime + SystemAPI.Time.DeltaTime) * 10000));
        var limitedMapLookup = SystemAPI.GetComponentLookup<LimitedMap>(true);
        foreach(var (mapViewer, mapViewerTransform) in SystemAPI.Query<MapViewer, LocalTransform>())
        {
            var mapViewerTilePosition = (int) math.floor(mapViewerTransform.Position.z / MapTilePrefab.TILE_WIDTH);
            var lastVisibleTile = mapViewerTilePosition + mapViewer.VisibleTilesCount;
            foreach(var (map, mapTiles, tileConfigs, tilePrefabs, mapEntity) in SystemAPI.Query<RefRW<Map>, DynamicBuffer<MapTile>, MapTileConfigs, DynamicBuffer<MapTilePrefab>>().WithEntityAccess())
            {
                var maxTilesCount = int.MaxValue;
                if (limitedMapLookup.TryGetComponent(mapEntity, out var limitedMap))
                    maxTilesCount = limitedMap.TilesCount;
                
                if(mapTiles.Length < lastVisibleTile && mapTiles.Length < maxTilesCount)
                {
                    var entityCommandBufferSystem = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
                    var entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged);

                    ref var configs = ref tileConfigs.BlobRefeence.Value;
                    var remainingTilesToSpawn = math.min(lastVisibleTile, maxTilesCount) - mapTiles.Length;
                    var lastTile = MapTile.GetLastTile(mapTiles);
                    while(remainingTilesToSpawn > 0)
                    {
                        SpawnTile(ref lastTile, ref map.ValueRW, mapTiles, ref configs, tilePrefabs, ref remainingTilesToSpawn, ref rng, entityCommandBuffer);
                    }
                }
            }
        }
    }

    [BurstCompile]
    private void SpawnTile(ref TileType lastTile, ref Map map, DynamicBuffer<MapTile> mapTiles, ref MapTileConfigsArray configs, DynamicBuffer<MapTilePrefab> tilePrefabs, ref int remainingTilesToSpawn, ref Random rng, EntityCommandBuffer entityCommandBuffer)
    {
        if(lastTile == 0)
            lastTile = TileType.Grass;

        MapTileConfig? lastTileConfig = configs.GetTileConfig(lastTile);

        if(!lastTileConfig.HasValue)
            return;

        var randomTile = lastTileConfig.Value.GetRandomNextTile(ref map.Rng);
        var randomTileConfig = configs.GetTileConfig(randomTile);
        var randomTilePrefab = MapTilePrefab.GetTilePrefab(tilePrefabs, randomTile);
        var tilesCount = configs.GetRandomConsecutiveTilesCount(randomTileConfig.Value, ref map.Rng);
        lastTile = randomTile;

        remainingTilesToSpawn -= tilesCount;

        for(var i = 0; i < tilesCount; i++)
        {
            var tileIndex = mapTiles.Length;
            mapTiles.Add(new MapTile { Tile = randomTile });
            var tilePosition = new float3(0, 0, tileIndex * MapTilePrefab.TILE_WIDTH);
            
            var spawnedTile = entityCommandBuffer.Instantiate(randomTilePrefab.Value.Prefab);
            var transformMatrix = float4x4.Translate(tilePosition);
            entityCommandBuffer.SetComponent(spawnedTile, LocalTransform.FromMatrix(transformMatrix));
            entityCommandBuffer.SetComponent(spawnedTile, WorldTransform.FromMatrix(transformMatrix));
            entityCommandBuffer.SetComponent(spawnedTile, new LocalToWorld { Value = transformMatrix });
            if(randomTile == TileType.Road)
            {
                entityCommandBuffer.SetComponent(spawnedTile, new RoadTile {
                    JustSpawned = true,
                    LastSpawnedDynamicObstacle = Entity.Null,
                    Speed = rng.NextFloat(20, 30) * (rng.NextBool() ? 1 : -1)
                });
            }
            else if(randomTile == TileType.Water)
            {
                entityCommandBuffer.SetComponent(spawnedTile, new WaterTile {
                    JustSpawned = true,
                    LastSpawnedDynamicObstacle = Entity.Null,
                    Speed = rng.NextFloat(3, 5) * (rng.NextBool() ? 1 : -1)
                });
            }
            else if(randomTile == TileType.Grass)
            {
                entityCommandBuffer.SetComponent(spawnedTile, new GrassTile {
                    JustSpawned = true,
                });
            }
        }
    }
}