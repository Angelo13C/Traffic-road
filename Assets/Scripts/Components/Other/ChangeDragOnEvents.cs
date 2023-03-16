using Unity.Entities;

public struct ChangeDragOnCollision : IComponentData
{
    public float NewDrag;
}

public struct ChangeDragOnHitByExplosion : IComponentData
{
    public float NewDrag;
}

public struct ChangeDragOnBlackHole : IComponentData
{
    public float NewDrag;
}