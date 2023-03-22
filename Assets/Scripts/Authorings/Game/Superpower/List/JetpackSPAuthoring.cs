using Unity.Entities;
using UnityEngine;

public class JetpackSPAuthoring : MonoBehaviour
{
    [SerializeField] private JetpackSP _config;
    
    class Baker : Baker<JetpackSPAuthoring>
    {
        public override void Bake(JetpackSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}