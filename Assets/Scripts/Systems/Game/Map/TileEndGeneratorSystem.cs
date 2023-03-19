using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct TileEndGeneratorSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (tileEndGenerator, mapTiles) in SystemAPI.Query<RefRW<TileEndGenerator>, DynamicBuffer<MapTile>>())
        {
            if(mapTiles.Length > tileEndGenerator.ValueRO.LastGeneratedTile)
            {
                for(var i = tileEndGenerator.ValueRO.LastGeneratedTile; i < mapTiles.Length; i++)
                {
                    TileEndGenerator.EndPrefabs endPrefabs;
                    if(mapTiles[i].Tile == TileType.Grass)
                        endPrefabs = tileEndGenerator.ValueRO.GrassTileEndPrefabs;
                    else if(mapTiles[i].Tile == TileType.Road)
                        endPrefabs = tileEndGenerator.ValueRO.RoadTileEndPrefabs;
                    else
                        endPrefabs = tileEndGenerator.ValueRO.WaterTileEndPrefabs;

                    SpawnEnds(state.EntityManager, endPrefabs.Continuation, endPrefabs.Continuation, i, 0f);

                    var before = i > 0 && mapTiles[i - 1].Tile == mapTiles[i].Tile;
                    var after = i + 1 != mapTiles.Length && mapTiles[i + 1].Tile == mapTiles[i].Tile;
                    if(!before)
                        SpawnEnds(state.EntityManager, endPrefabs.End, endPrefabs.Start, i, -0.5f);
                    if(!after)
                        SpawnEnds(state.EntityManager, endPrefabs.Start, endPrefabs.End, i, 0.5f);
                }
                tileEndGenerator.ValueRW.LastGeneratedTile = mapTiles.Length;
            }
        }
    }

    [BurstCompile]
    private void SpawnEnds(EntityManager entityManager, Entity prefabPositive, Entity prefabNegative, int tile, float tileOffset)
    {
        var position = new float3(MapTilePrefab.TILE_LENGTH / 2, 0f, MapTilePrefab.TILE_WIDTH * (tile + tileOffset));
        if(prefabPositive != Entity.Null)
            SpawnEnd(entityManager, prefabPositive, position, quaternion.EulerXYZ(math.radians(-90), math.radians(180), 0));
        position.x = -position.x;
        if(prefabNegative != Entity.Null)
            SpawnEnd(entityManager, prefabNegative, position, quaternion.EulerXYZ(math.radians(-90), 0, 0));
    }

    [BurstCompile]
    private void SpawnEnd(EntityManager entityManager, Entity prefab, float3 position, quaternion rotation)
    {
        var spawned = entityManager.Instantiate(prefab);
        var transform = new LocalTransform {
            Position = position,
            Rotation = rotation,
            Scale = 1f
        };
        entityManager.SetComponentData(spawned, transform);
    }
}