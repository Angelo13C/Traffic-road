using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(MapTileSpawnerSystem))]
[BurstCompile]
public partial struct MapDynamicObstaclesSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var rng = new Random(1 + (uint) ((SystemAPI.Time.ElapsedTime + SystemAPI.Time.DeltaTime) * 10000));
        var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        foreach(var (roadObstaclesConfig, roadDynamicObstacles) in SystemAPI.Query<RoadObstaclesConfig, DynamicBuffer<RoadDynamicObstacles>>())
        {
            foreach(var (roadTile, transform) in SystemAPI.Query<RefRW<RoadTile>, LocalTransform>())
            {
                SpawnObstacleIfNecessary<RoadTile, RoadDynamicObstacles>(transformLookup, ref roadTile.ValueRW, roadDynamicObstacles, roadObstaclesConfig.TotalWeight, roadObstaclesConfig.DistanceRangeBetweenObstacles, state.EntityManager, transform.Position, ref rng);
            }
        }
        
        foreach(var (waterObstaclesConfig, waterDynamicObstacles) in SystemAPI.Query<WaterObstaclesConfig, DynamicBuffer<WaterDynamicObstacles>>())
        {
            foreach(var (waterTile, transform) in SystemAPI.Query<RefRW<WaterTile>, LocalTransform>())
            {
                SpawnObstacleIfNecessary<WaterTile, WaterDynamicObstacles>(transformLookup, ref waterTile.ValueRW, waterDynamicObstacles, waterObstaclesConfig.TotalWeight, waterObstaclesConfig.DistanceRangeBetweenObstacles, state.EntityManager, transform.Position, ref rng);
            }
        }
    }

    [BurstCompile]
    private void SpawnObstacleIfNecessary<T, B>(ComponentLookup<LocalTransform> transformLookup, ref T tile, DynamicBuffer<B> obstacles, int totalWeight, float2 distanceRangeBetweenObstacles, EntityManager entityManager, float3 tilePosition, ref Random rng)
        where T : unmanaged, TileWithDynamicObstacles where B : unmanaged, ObstaclesBuffer
    {
        var shouldSpawn = tile.LastSpawnedDynamicObstacle == Entity.Null;
        if(!shouldSpawn)
        {
            if(transformLookup.TryGetComponent(tile.LastSpawnedDynamicObstacle, out var lastSpawnedObstacleTransform))
                shouldSpawn = math.abs(lastSpawnedObstacleTransform.Position.x) <= tile.NextAbsXPositionToSpawnObstacle;
        }
        if(shouldSpawn)
        {
            tile.NextAbsXPositionToSpawnObstacle = MapTilePrefab.TILE_LENGTH / 2 - rng.NextFloat(distanceRangeBetweenObstacles.x, distanceRangeBetweenObstacles.y);
            tile.LastSpawnedDynamicObstacle = SpawnObstacle(obstacles, totalWeight, entityManager, tilePosition, tile.Speed, ref rng);
        }
    }

    [BurstCompile]
    private Entity SpawnObstacle<T>(DynamicBuffer<T> obstacles, int totalWeight, EntityManager entityManager, float3 tilePosition, float speed, ref Random rng)
        where T : unmanaged, ObstaclesBuffer
    {
        var angle = math.radians(-90) * math.sign(speed);
        var spawnPosition = math.sign(speed) * MapTilePrefab.TILE_LENGTH / 2;
        var randomValue = rng.NextInt(totalWeight);
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

        var spawnedObstacle = entityManager.Instantiate(randomPrefab);
        entityManager.SetComponentData(spawnedObstacle, new LocalTransform {
            Position = new float3(spawnPosition, tilePosition.y, tilePosition.z),
            Rotation = quaternion.RotateY(angle),
            Scale = 1
        });
        entityManager.SetComponentData(spawnedObstacle, new VehicleMover {
            Speed = speed
        });
        return spawnedObstacle;
    }
}