using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[System.Flags]
public enum Tile
{
    Grass = (1 << 0),
    Road = (1 << 1),
    Water = (1 << 2)
}

public struct MapTileConfigs : IComponentData
{
    public BlobAssetReference<MapTileConfigsArray> BlobRefeence;
}

public struct MapTileConfigsArray
{
    public BlobArray<MapTileConfig> Configs;
    public BlobArray<int> ConsecutiveTilesSpawnRateWeight;

    public MapTileConfig? GetTileConfig(Tile tile)
    {
        for(var i = 0; i < Configs.Length; i++)
        {
            if(Configs[i].Tile == tile)
            {
                return Configs[i];
            }
        }
        return null;
    }

    [BurstCompile]
    public int GetRandomConsecutiveTilesCount(MapTileConfig config, ref Random rng)
    {
        var randomValue = rng.NextInt(config.TotalConsecutiveTilesSpawnRateWeights);
        var offset = config.ConsecutiveTilesSpawnRateWeightStartIndex;
        for(var i = 0; i < config.ConsecutiveTilesSpawnRateWeightEndIndex - offset; i++)
        {
            if(randomValue < ConsecutiveTilesSpawnRateWeight[offset + i])
                return i + 1;

            randomValue -= ConsecutiveTilesSpawnRateWeight[offset + i];
        }
        return -1;
    }
}

[BurstCompile]
public struct MapTileConfig
{
    public Tile Tile;
    public Tile AllowedNextTiles;

    public int SpawnRateWeight;
    public int ConsecutiveTilesSpawnRateWeightStartIndex;
    public int ConsecutiveTilesSpawnRateWeightEndIndex;
    public int TotalConsecutiveTilesSpawnRateWeights;

    [BurstCompile]
    public Tile GetRandomNextTile(ref Random rng)
    {
        var allowedNextTilesCount = math.countbits((int) AllowedNextTiles);
        var chosenNextTile = (int) AllowedNextTiles;
        
        for (int i = rng.NextInt(allowedNextTilesCount); i > 0; i--) {
            chosenNextTile &= chosenNextTile - 1; // remove the least significant bit
        }
        return (Tile) (chosenNextTile & ~(chosenNextTile - 1));
    }
}