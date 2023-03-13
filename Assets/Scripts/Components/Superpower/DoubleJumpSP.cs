using Unity.Entities;
using UnityEngine;

[System.Serializable]
public struct DoubleJumpSP : IComponentData
{
    public float Duration;
    public bool HasFinished => Duration <= 0;
    public float Force;

    [HideInInspector] public byte CurrentInAirJumpsCount;
    public bool CanDoubleJump => CurrentInAirJumpsCount == 1;
}