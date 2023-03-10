using Unity.Entities;
using Unity.Mathematics;

public struct Map : IComponentData
{
    public Random Rng;
}

[InternalBufferCapacity(0)]
public struct MapTile : IBufferElementData
{
    public Tile Tile;

    public static Tile GetLastTile(DynamicBuffer<MapTile> tiles) => tiles.Length == 0 ? 0 : tiles[tiles.Length - 1].Tile;
}

[InternalBufferCapacity(8)]
public struct MapTilePrefab : IBufferElementData
{
    public Tile Tile;
    public Entity Prefab;

    public const float TILE_WIDTH = 3;

    public static MapTilePrefab? GetTilePrefab(DynamicBuffer<MapTilePrefab> prefabs, Tile tile)
    {
        for(var i = 0; i < prefabs.Length; i++)
        {
            if(prefabs[i].Tile == tile)
                return prefabs[i];
        }
        return null;
    }
}