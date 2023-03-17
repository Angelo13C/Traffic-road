using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct RoadMarkingsGeneratorSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (roadMarkingsGenerator, tiles) in SystemAPI.Query<RefRW<RoadMarkingsGenerator>, DynamicBuffer<MapTile>>())
        {
            for(var i = roadMarkingsGenerator.ValueRO.LastSpawnedTileIndex; i < tiles.Length - 1; i++)
            {
                if(tiles[i].Tile == TileType.Road && tiles[i + 1].Tile == TileType.Road)
                {
                    var spawnedRoadMarkings = state.EntityManager.Instantiate(roadMarkingsGenerator.ValueRO.RoadMarkingsPrefab);
                    SystemAPI.SetComponent(spawnedRoadMarkings, new LocalTransform {
                        Position = new float3(100, 0.151f, i * MapTilePrefab.TILE_WIDTH + MapTilePrefab.TILE_WIDTH / 2),
                        Rotation = quaternion.EulerXYZ(math.radians(-90), 0, 0),
                        Scale = 1f
                    });
                }
            }
            roadMarkingsGenerator.ValueRW.LastSpawnedTileIndex = math.max(0, tiles.Length - 1);
        }
    }
}