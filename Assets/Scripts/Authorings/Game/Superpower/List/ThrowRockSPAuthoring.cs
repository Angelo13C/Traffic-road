using Unity.Entities;
using UnityEngine;

public class ThrowRockSPAuthoring : MonoBehaviour
{
    [SerializeField] private ThrowRockSP _config;
    
    class Baker : Baker<ThrowRockSPAuthoring>
    {
        public override void Bake(ThrowRockSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}