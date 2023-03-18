using Unity.Entities;
using Unity.Physics.Authoring;

public struct AttractBodies : IComponentData
{
    public float Range;
    public float Force;
    public PhysicsCategoryTags AttractableBodiesTags;
}