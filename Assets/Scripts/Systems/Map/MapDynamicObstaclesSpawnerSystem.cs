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
                var shouldSpawn = roadTile.ValueRO.LastSpawnedDynamicObstacle == Entity.Null;
                if(!shouldSpawn)
                {
                    var lastSpawnedObstacleTransform = transformLookup.GetRefRO(roadTile.ValueRO.LastSpawnedDynamicObstacle);
                    shouldSpawn = math.abs(lastSpawnedObstacleTransform.ValueRO.Position.x) <= roadTile.ValueRO.NextAbsXPositionToSpawnObstacle;
                }
                var speedDirection = math.sign(roadTile.ValueRO.Speed);
                if(shouldSpawn)
                {
                    roadTile.ValueRW.NextAbsXPositionToSpawnObstacle = MapTilePrefab.TILE_LENGTH / 2 - rng.NextFloat(roadObstaclesConfig.DistanceRangeBetweenObstacles.x, roadObstaclesConfig.DistanceRangeBetweenObstacles.y);
                    roadTile.ValueRW.LastSpawnedDynamicObstacle = SpawnObstacle(roadDynamicObstacles, roadObstaclesConfig.TotalWeight, state.EntityManager, transform.Position, roadTile.ValueRO.Speed, ref rng);
                }
            }
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