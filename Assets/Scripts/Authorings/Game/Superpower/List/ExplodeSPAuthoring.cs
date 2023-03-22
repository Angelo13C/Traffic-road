using Unity.Entities;
using UnityEngine;

public class ExplodeSPAuthoring : MonoBehaviour
{
    [SerializeField] private ExplodeSP _config;
    
    class Baker : Baker<ExplodeSPAuthoring>
    {
        public override void Bake(ExplodeSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}