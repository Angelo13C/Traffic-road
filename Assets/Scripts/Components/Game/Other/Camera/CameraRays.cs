using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
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