using Unity.Entities;

[System.Serializable]
public struct BlackHoleSP : IComponentData
{
    public float SpawnDistanceFromEye;
    public float ThrowSpeed;
}