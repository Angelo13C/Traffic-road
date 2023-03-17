using Unity.Entities;

public struct RoadMarkingsGenerator : IComponentData
{
    public Entity RoadMarkingsPrefab;
    public int LastSpawnedTileIndex;
}