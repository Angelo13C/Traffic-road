using Unity.Entities;

[System.Serializable]
public struct TimeFreezeSP : IComponentData
{
    public float Duration;
    public bool HasFinished => Duration <= 0;
}