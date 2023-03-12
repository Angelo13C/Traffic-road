using Unity.Entities;

[System.Flags]
public enum TileType
{
    Grass = (1 << 0),
    Road = (1 << 1),
    Water = (1 << 2)
}

public struct GrassTile : IComponentData
{
    public bool JustSpawned;

    public const float HEIGHT = 0.5f;
}

public interface TileWithDynamicObstacles : IComponentData
{
    public float Speed { get; set; }
    public Entity LastSpawnedDynamicObstacle { get; set; }
    public float NextAbsXPositionToSpawnObstacle { get; set; }
}

public struct RoadTile : TileWithDynamicObstacles, IComponentData
{
    public bool JustSpawned;
    public float Speed { get; set; }
    public Entity LastSpawnedDynamicObstacle { get; set; }
    public float NextAbsXPositionToSpawnObstacle { get; set; }
}

public struct WaterTile : TileWithDynamicObstacles, IComponentData
{
    public bool JustSpawned;
    public float Speed { get; set; }
    public Entity LastSpawnedDynamicObstacle { get; set; }
    public float NextAbsXPositionToSpawnObstacle { get; set; }
}