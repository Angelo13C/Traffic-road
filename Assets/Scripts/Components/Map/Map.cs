using Unity.Entities;

public struct Map : IComponentData
{

}

[InternalBufferCapacity(8)]
public struct MapTilePrefab : IBufferElementData
{
    public Tile Tile;
    public Entity Prefab;

    public const float TILE_WIDTH = 3;
}