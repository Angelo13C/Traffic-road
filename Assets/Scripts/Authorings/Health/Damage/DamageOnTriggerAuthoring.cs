using Unity.Entities;
using UnityEngine;

public class DamageOnTriggerAuthoring : MonoBehaviour
{
    [SerializeField] [Min(0)] private float _minVelocityToDamage;
    [SerializeField] [Min(0)] private int _damageToDealForMinVelocity;

	class Baker : Baker<DamageOnTriggerAuthoring>
	{
		public override void Bake(DamageOnTriggerAuthoring authoring)
		{
			var damageOnCollision = new DamageOnTrigger {
				MinVelocityToDamageSqr = authoring._minVelocityToDamage * authoring._minVelocityToDamage,
				DamageToDealForMinVelocity = authoring._damageToDealForMinVelocity
			};

			AddComponent(damageOnCollision);
		}
	}
}