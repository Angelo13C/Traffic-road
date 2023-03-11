using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;

[BurstCompile]
public partial struct MapStaticObstaclesSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var rng = new Random((uint) ((SystemAPI.Time.ElapsedTime + SystemAPI.Time.DeltaTime) * 10000));
        foreach(var (grassObstaclesConfig, grassStaticObstacles) in SystemAPI.Query<GrassObstaclesConfig, DynamicBuffer<GrassStaticObstacles>>())
        {
            foreach(var (grassTile, transform) in SystemAPI.Query<RefRW<GrassTile>, LocalTransform>())
            {
                if(grassTile.ValueRO.JustSpawned)
                {
                    grassTile.ValueRW.JustSpawned = false;
                    SpawnObstacles(grassObstaclesConfig, grassStaticObstacles, ref rng, state.EntityManager, transform.Position);
                }
            }
        }
    }

    [BurstCompile]
    private void SpawnObstacles<B, C>(C config, DynamicBuffer<B> obstacles, ref Random rng, EntityManager entityManager, float3 tilePosition)
        where B : unmanaged, ObstaclesBuffer where C : unmanaged, ObstaclesConfig
    {
        var occupiedTiles = new NativeArray<bool>(MapTilePrefab.TILE_SLOTS_COUNT, Allocator.Temp);
        
        var obstaclesToSpawnCount = rng.NextInt(config.ObstaclesCount.x, config.ObstaclesCount.y);
        for(var i = 0; i < obstaclesToSpawnCount; i++)
        {
            var randomValue = rng.NextInt(config.TotalWeight);
            var randomPrefab = Entity.Null;
            for(var j = 0; j < obstacles.Length; j++)
            {
                if(randomValue < obstacles[j].Weight)
                {
                    randomPrefab = obstacles[j].Prefab;
                    break;
                }

                randomValue -= obstacles[j].Weight;
            }
            int spawnTileIndex;
            do
            {
                spawnTileIndex = rng.NextInt(occupiedTiles.Length);
            } while(occupiedTiles[spawnTileIndex]);
            occupiedTiles[spawnTileIndex] = true;

            var spawnTileXPosition = spawnTileIndex * MapTilePrefab.TILE_WIDTH - MapTilePrefab.TILE_LENGTH / 2;
            var spawnedObstacle = entityManager.Instantiate(randomPrefab);
            entityManager.SetComponentData(spawnedObstacle, new LocalTransform {
                Position = new float3(spawnTileXPosition, tilePosition.y, tilePosition.z),
                Rotation = quaternion.identity,
                Scale = 1
            });
        }

        occupiedTiles.Dispose();
    }
}