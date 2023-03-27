using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
	[SerializeField] [Min(0)] private int _maxHealth;
	[SerializeField] private bool _destroyOnDeath = true;

	class Baker : Baker<HealthAuthoring>
	{
		public override void Bake(HealthAuthoring authoring)
		{
			var health = new Health {
				Current = authoring._maxHealth,
			};

			AddComponent(health);
			
			if(authoring._destroyOnDeath)
				AddComponent<DestroyOnDeath>();
		}
	}
}