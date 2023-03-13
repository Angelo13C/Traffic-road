using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class DamageOnCollisionAuthoring : MonoBehaviour
{
    [SerializeField] [Min(0)] private float _minForceToDamage;
    [SerializeField] [Min(0)] private int _damageToDealForMinForce;
	[SerializeField] private CustomPhysicsBodyTags _bodiesThatCanDamage;

	class Baker : Baker<DamageOnCollisionAuthoring>
	{
		public override void Bake(DamageOnCollisionAuthoring authoring)
		{
			var damageOnCollision = new DamageOnCollision {
				MinForceToDamage = authoring._minForceToDamage,
				DamageToDealForMinForce = authoring._damageToDealForMinForce,
				BodiesThatCanDamageTags = authoring._bodiesThatCanDamage.Value
			};

			AddComponent(damageOnCollision);
		}
	}
}