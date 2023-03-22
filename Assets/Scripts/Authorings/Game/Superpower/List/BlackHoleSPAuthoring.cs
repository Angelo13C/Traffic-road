using Unity.Entities;
using UnityEngine;

public class BlackHoleSPAuthoring : MonoBehaviour
{
    [SerializeField] private BlackHoleSP _config;
    
    class Baker : Baker<BlackHoleSPAuthoring>
    {
        public override void Bake(BlackHoleSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}