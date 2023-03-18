using Unity.Entities;

public struct Score : IComponentData
{
    public int Current;
}

public struct ScoreOnTravel : IComponentData
{
    public int HighestTileIndex;
}