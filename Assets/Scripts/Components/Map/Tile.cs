using Unity.Entities;

[System.Flags]
public enum TileType
{
    Grass = (1 << 0),
    Road = (1 << 1),
    Water = (1 << 2)
}