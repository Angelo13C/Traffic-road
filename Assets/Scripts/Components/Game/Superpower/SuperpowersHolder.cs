using Unity.Entities;

public struct SuperpowerUser : IComponentData
{
    public Entity LastUsedSuperpower;
}

public struct HeldSuperpower : IBufferElementData
{
    public Entity Prefab;
}