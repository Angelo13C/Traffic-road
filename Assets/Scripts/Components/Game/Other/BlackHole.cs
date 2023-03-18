using Unity.Entities;
using Unity.Physics.Authoring;

public struct BlackHole : IComponentData
{
    public float LastScale;
    
    public PhysicsCategoryTags BecomeStaticOnTouchTags;
    public float BecomeStaticRadius;
    public float BecomeStaticRadiusGrowthPerSecond;
}