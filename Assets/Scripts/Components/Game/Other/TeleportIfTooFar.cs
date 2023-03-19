using Unity.Entities;

public struct TeleportIfTooFar : IComponentData
{
    public Entity From;
    public float MaxZDistance;
}