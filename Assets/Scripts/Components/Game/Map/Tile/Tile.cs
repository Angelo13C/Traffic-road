using Unity.Entities;

[System.Flags]
public enum TileType
{
    Grass = (1 << 0),
    Road = (1 << 1),
    Water = (1 << 2)
}

public interface Tile : IComponentData
{
    public bool JustSpawned { get; set; }
}

public struct GrassTile : IComponentData, Tile
{
    public bool JustSpawned { get; set; }

    public const float HEIGHT = 0.5f;
}

public interface TileWithDynamicObstacles : Tile
{
    public float Speed { get; set; }
    public Entity LastSpawnedDynamicObstacle { get; set; }
    public float NextAbsXPositionToSpawnObstacle { get; set; }
}

public struct RoadTile : TileWithDynamicObstacles, IComponentData, Tile
{
    public bool JustSpawned { get; set; }
    public float Speed { get; set; }
    public Entity LastSpawnedDynamicObstacle { get; set; }
    public float NextAbsXPositionToSpawnObstacle { get; set; }
}

public struct WaterTile : TileWithDynamicObstacles, IComponentData, Tile
{
    public bool JustSpawned { get; set; }
    public float Speed { get; set; }
    public Entity LastSpawnedDynamicObstacle { get; set; }
    public float NextAbsXPositionToSpawnObstacle { get; set; }
}