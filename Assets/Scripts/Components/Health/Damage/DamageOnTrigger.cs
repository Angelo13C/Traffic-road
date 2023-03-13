using Unity.Entities;

public struct DamageOnTrigger : IComponentData
{
    public float MinVelocityToDamageSqr;
    public int DamageToDealForMinVelocity;
}