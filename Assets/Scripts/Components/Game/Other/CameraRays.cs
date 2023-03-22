using Unity.Entities;
using UnityEngine;

public struct CameraRays : IComponentData
{
    public Ray Mouse;
    public Ray ScreenCenter;
}

public class CameraReference : IComponentData
{
    public Camera Camera;
}