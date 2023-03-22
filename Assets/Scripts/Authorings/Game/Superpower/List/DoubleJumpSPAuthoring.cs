using Unity.Entities;
using UnityEngine;

public class DoubleJumpSPAuthoring : MonoBehaviour
{
    [SerializeField] private DoubleJumpSP _config;
    
    class Baker : Baker<DoubleJumpSPAuthoring>
    {
        public override void Bake(DoubleJumpSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}