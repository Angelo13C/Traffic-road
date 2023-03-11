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
}

public struct RoadTile : IComponentData
{
    public bool JustSpawned;
    public float Speed;
    public Entity LastSpawnedDynamicObstacle;
    public float NextAbsXPositionToSpawnObstacle;
}

public struct WaterTile : IComponentData
{
    public bool JustSpawned;
    public Entity LastSpawnedDynamicObstacle;
}