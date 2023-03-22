using Unity.Entities;
using UnityEngine;

public class TimeFreezeSPAuthoring : MonoBehaviour
{
    [SerializeField] private TimeFreezeSP _config;
    
    class Baker : Baker<TimeFreezeSPAuthoring>
    {
        public override void Bake(TimeFreezeSPAuthoring authoring)
        {
            AddComponent(authoring._config);
        }
    }
}