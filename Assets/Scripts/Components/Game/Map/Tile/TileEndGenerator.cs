using Unity.Entities;

public struct TileEndGenerator : IComponentData
{
    public int LastGeneratedTile;

    public EndPrefabs RoadTileEndPrefabs;
    public EndPrefabs GrassTileEndPrefabs;
    public EndPrefabs WaterTileEndPrefabs;

    public struct EndPrefabs
    {
        public Entity Start;
        public Entity Continuation;
        public Entity End;
    }
}