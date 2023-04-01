using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial struct UpdateCameraRaysSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        foreach (var cameraReferenceEntity in SystemAPI.QueryBuilder().WithAll<CameraRays>().WithNone<CameraReference>()
                     .Build().ToEntityArray(Allocator.Temp))
        {
            entityCommandBuffer.AddComponent(cameraReferenceEntity, new CameraReference
            {
                Camera = Camera.main
            });
        }
        entityCommandBuffer.Playback(state.EntityManager);
        
        foreach (var (cameraReference, cameraRays) in SystemAPI.Query<CameraReference, RefRW<CameraRays>>())
        {
            cameraRays.ValueRW.Mouse = cameraReference.Camera.ScreenPointToRay(Input.mousePosition);
            cameraRays.ValueRW.ScreenCenter = cameraReference.Camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        }
    }
}