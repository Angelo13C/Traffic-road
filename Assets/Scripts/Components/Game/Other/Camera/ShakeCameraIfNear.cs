using CameraShake;
using Unity.Entities;

public struct ShakeCameraIfNear : IComponentData, IEnableableComponent
{
    public float ShakeForce;
    public float ShakeDuration;
    public bool SingleShake;
    
    
    public float ClippingDistance;
    public float FalloffScale;
    public Degree FalloffDegree;
}