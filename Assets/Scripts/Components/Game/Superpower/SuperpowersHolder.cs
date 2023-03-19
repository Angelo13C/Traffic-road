using Unity.Entities;

[System.Serializable]
public struct SuperpowersHolder : IComponentData
{
    public DoubleJumpSP DoubleJump;
    public JetpackSP Jetpack;
    public TimeFreezeSP TimeFreeze;
    public TeleportSP Teleport;
    public SuperSpeedSP SuperSpeed;
    public ExplodeSP Explode;
    public ThrowRockSP ThrowRock;
    public BlackHoleSP BlackHole;
}