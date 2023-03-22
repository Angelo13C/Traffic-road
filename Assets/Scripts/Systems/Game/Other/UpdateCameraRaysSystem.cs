using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public partial struct UpdateCameraRaysSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (cameraReference, cameraRays) in SystemAPI.Query<CameraReference, RefRW<CameraRays>>())
        {
            cameraRays.ValueRW.Mouse = cameraReference.Camera.ScreenPointToRay(Input.mousePosition);
            cameraRays.ValueRW.ScreenCenter = cameraReference.Camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        }
    }
}