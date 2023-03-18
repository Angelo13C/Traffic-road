using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct ScoreOnTravelSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (score, scoreOnTravel, transform) in SystemAPI.Query<RefRW<Score>, RefRW<ScoreOnTravel>, LocalTransform>())
        {
            var currentTileIndex = (int) math.floor(transform.Position.z / MapTilePrefab.TILE_WIDTH);
            var travelledTiles = currentTileIndex - scoreOnTravel.ValueRO.HighestTileIndex;
            if(travelledTiles > 0)
            {
                score.ValueRW.Current += travelledTiles;
                scoreOnTravel.ValueRW.HighestTileIndex = currentTileIndex;
            }
        }
    }
}