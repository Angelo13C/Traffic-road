using Unity.Entities;
using UnityEngine;

public struct CameraRays : IComponentData
{
    public Ray Mouse;
    public Ray ScreenCenter;
}