using Unity.Entities;
using UnityEngine;

public class TeleportSPAuthoring : MonoBehaviour
{
    [SerializeField] private TeleportSP _config;
    
    class Baker : Baker<TeleportSPAuthoring>
    {
        public override void Bake(TeleportSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}