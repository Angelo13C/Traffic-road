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
}

[BurstCompile]
public struct MapTileConfig
{
    public Tile Tile;
    public Tile AllowedNextTiles;

    public int SpawnRateWeight;
    public BlobArray<int> ConsecutiveTilesSpawnRateWeight;
    [UnityEngine.HideInInspector] public int _totalConsecutiveTilesSpawnRateWeights;

    [BurstCompile]
    public int GetRandomConsecutiveTilesCount(ref Random rng)
    {
        var randomValue = rng.NextInt(_totalConsecutiveTilesSpawnRateWeights);
        for(var i = 0; i < ConsecutiveTilesSpawnRateWeight.Length; i++)
        {
            if(randomValue < ConsecutiveTilesSpawnRateWeight[i])
                return i + 1;

            randomValue -= ConsecutiveTilesSpawnRateWeight[i];
        }
        return -1;
    }

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