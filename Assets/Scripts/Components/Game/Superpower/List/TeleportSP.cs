using Unity.Entities;
using Unity.Physics.Authoring;

[System.Serializable]
public struct TeleportSP : IComponentData
{
    public float MaxDistance;
    public PhysicsCategoryTags HittableFilter;
}