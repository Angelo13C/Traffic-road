using Unity.Entities;
using UnityEngine;

public class VehicleMoverAuthoring : MonoBehaviour
{
	[SerializeField] [Range(0, 50)] private float _speed = 30;

	class Baker : Baker<VehicleMoverAuthoring>
	{
		public override void Bake(VehicleMoverAuthoring authoring)
		{
			var vehicleMover = new VehicleMover {
				Speed = authoring._speed
			};

			AddComponent(vehicleMover);
		}
	}
}