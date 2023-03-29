using CameraShake;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct ShakeCameraIfNearSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var cameraShakeListenerTransform in SystemAPI.Query<LocalTransform>().WithAll<CameraShakeListener>())
        {
            foreach (var (shakeCameraIfNear, shakerTransform, shakerEntity) in SystemAPI.Query<ShakeCameraIfNear, LocalTransform>().WithEntityAccess())
            {
                if(shakeCameraIfNear.SingleShake)
                    SystemAPI.SetComponentEnabled<ShakeCameraIfNear>(shakerEntity, false);

                var strengthMultiplier = Attenuator.Strength(new Attenuator.StrengthAttenuationParams
                {
                    clippingDistance = shakeCameraIfNear.ClippingDistance,
                    falloffScale = shakeCameraIfNear.FalloffScale,
                    falloffDegree = shakeCameraIfNear.FalloffDegree,
                    axesMultiplier = Vector3.one
                }, shakerTransform.Position, cameraShakeListenerTransform.Position);
                CameraShaker.Presets.Explosion3D(strengthMultiplier * shakeCameraIfNear.ShakeForce, shakeCameraIfNear.ShakeDuration);
            }
        }
    }
}