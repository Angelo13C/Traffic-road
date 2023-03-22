using Unity.Entities;
using UnityEngine;

public class SuperSpeedSPAuthoring : MonoBehaviour
{
    [SerializeField] private SuperSpeedSP _config;
    
    class Baker : Baker<SuperSpeedSPAuthoring>
    {
        public override void Bake(SuperSpeedSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}