using Unity.Entities;

[System.Serializable]
public struct JetpackSP : IComponentData
{
    public float Duration;
    public bool HasFinished => Duration <= 0;
    public float Force;
    public float MaxHeight;
}