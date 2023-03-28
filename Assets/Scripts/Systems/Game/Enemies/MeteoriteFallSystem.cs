using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct MeteoriteFallSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var rng = new Random(1 + (uint) ((SystemAPI.Time.ElapsedTime + deltaTime) * 10000));
        var maxZViewed = 0f;
        var targetZPosition = 0f;
        foreach (var (mapViewer, transform) in SystemAPI.Query<MapViewer, LocalTransform>())
        {
            maxZViewed = math.max(maxZViewed,
                transform.Position.z + mapViewer.VisibleTilesCount * MapTilePrefab.TILE_WIDTH);
            targetZPosition = transform.Position.z;
        }

        foreach (var meteoriteFall in SystemAPI.Query<RefRW<MeteoriteFall>>())
        {
            meteoriteFall.ValueRW.CurrentZ += meteoriteFall.ValueRO.ZChangePerSecond * deltaTime;

            if (meteoriteFall.ValueRO.CurrentZ < 0)
                continue;

            meteoriteFall.ValueRW.BehindConfig.RemainingTime -= deltaTime;
            while (meteoriteFall.ValueRO.BehindConfig.RemainingTime <= 0)
            {
                meteoriteFall.ValueRW.BehindConfig.RemainingTime += meteoriteFall.ValueRO.BehindConfig.TimeBetweenMeteorites;
                var meteorite = state.EntityManager.Instantiate(meteoriteFall.ValueRO.MeteoritePrefab);
                var spawnZBiased = meteoriteFall.ValueRO.CurrentZ;
                if (spawnZBiased > targetZPosition)
                    spawnZBiased = math.lerp(targetZPosition, spawnZBiased, 0.5f);
                var spawnZ = math.clamp(spawnZBiased - rng.NextFloat(20), 0, maxZViewed);
                var spawnX = rng.NextFloat(-MapTilePrefab.TILE_LENGTH / 2, MapTilePrefab.TILE_LENGTH / 2);
                SystemAPI.SetComponent(meteorite, new LocalTransform
                {
                    Position = new float3(spawnX, meteoriteFall.ValueRO.SpawnHeight, spawnZ),
                    Rotation = rng.NextQuaternionRotation(),
                    Scale = rng.NextFloat(0.7f, 1f)
                });
            }
        }
    }
}