using Unity.Entities;

[System.Serializable]
public struct SuperpowersHolder : IComponentData
{
    public DoubleJumpSP DoubleJump;
    public JetpackSP Jetpack;
    public TimeFreezeSP TimeFreeze;
}