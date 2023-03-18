using Unity.Entities;
using UnityEngine.UIElements;

public struct Score : IComponentData
{
    public int Current;
}

public struct ScoreOnTravel : IComponentData
{
    public int HighestTileIndex;
}

public class ScoreUI : IComponentData
{
    public int LastScore;
    public Label ScoreLabel;
}