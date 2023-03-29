using Unity.Entities;
using UnityEngine;

public class CameraShakeListenerAuthoring : MonoBehaviour
{
    class Baker : Baker<CameraShakeListenerAuthoring>
    {
        public override void Bake(CameraShakeListenerAuthoring authoring)
        {
            AddComponent<CameraShakeListener>();
        }
    }
}