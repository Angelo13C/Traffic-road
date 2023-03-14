using Unity.Entities;
using UnityEngine;

[System.Serializable]
public struct SuperSpeedSP : IComponentData
{
    public float Duration;
    public bool HasFinished => Duration <= 0;
    public float SpeedMultiplier;
    [HideInInspector] public bool HasBeenApplied;
}