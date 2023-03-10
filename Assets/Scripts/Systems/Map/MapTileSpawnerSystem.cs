using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct MapTileSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (mapViewer, mapViewerTransform) in SystemAPI.Query<MapViewer, LocalTransform>())
        {
            var mapViewerTilePosition = (int) math.floor(mapViewerTransform.Position.z / MapTilePrefab.TILE_WIDTH);
            var lastVisibleTile = mapViewerTilePosition + mapViewer.VisibleTilesCount;
            foreach(var (map, mapTiles, tileConfigs, tilePrefabs) in SystemAPI.Query<RefRW<Map>, DynamicBuffer<MapTile>, MapTileConfigs, DynamicBuffer<MapTilePrefab>>())
            {
                if(mapTiles.Length < lastVisibleTile)
                {
                    var entityCommandBufferSystem = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
                    var entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer(state.WorldUnmanaged);

                    ref var configs = ref tileConfigs.BlobRefeence.Value;
                    var remainingTilesToSpawn = lastVisibleTile - mapTiles.Length;
                    var lastTile = MapTile.GetLastTile(mapTiles);
                    while(remainingTilesToSpawn > 0)
                    {
                        SpawnTile(ref lastTile, ref map.ValueRW, mapTiles, ref configs, tilePrefabs, ref remainingTilesToSpawn, entityCommandBuffer);
                    }
                }
            }
        }
    }

    [BurstCompile]
    private void SpawnTile(ref Tile lastTile, ref Map map, DynamicBuffer<MapTile> mapTiles, ref MapTileConfigsArray configs, DynamicBuffer<MapTilePrefab> tilePrefabs, ref int remainingTilesToSpawn, EntityCommandBuffer entityCommandBuffer)
    {
        if(lastTile == 0)
            lastTile = Tile.Grass;

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
            entityCommandBuffer.SetComponent(spawnedTile, new LocalTransform {
                Position = tilePosition,
                Rotation = quaternion.identity,
                Scale = 1
            });
        }
    }
}