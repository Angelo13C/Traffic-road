using System;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class NearbyCarsTrackerAuthoring : MonoBehaviour
{
	[SerializeField] [Min(1)] private float _radius = 10;
	[SerializeField] private PhysicsCategoryTags _carBodyTag;
	[SerializeField] [Min(0)] private int _maxTrackedCarsCount = 5;

	class Baker : Baker<NearbyCarsTrackerAuthoring>
	{
		public override void Bake(NearbyCarsTrackerAuthoring authoring)
		{
			var nearbyCarsTracker = new NearbyCarsTracker
			{
				Radius = authoring._radius,
				CollisionFilter = new CollisionFilter
				{
					GroupIndex = CollisionFilter.Default.GroupIndex,
					BelongsTo = CollisionFilter.Default.BelongsTo,
					CollidesWith = authoring._carBodyTag.Value
				},
				MaxTrackedCarsCount = authoring._maxTrackedCarsCount
			};
			AddComponent(nearbyCarsTracker);

			AddBuffer<NearbyCars>();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, _radius);
	}
}