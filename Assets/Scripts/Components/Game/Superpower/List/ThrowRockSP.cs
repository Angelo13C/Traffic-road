using Unity.Entities;

[System.Serializable]
public struct ThrowRockSP : IComponentData
{
    public float DistanceFromEye;
    public float ThrowSpeed;
}