using Unity.Entities;
using UnityEngine;

public class TeleportIfTooFarAuthoring : MonoBehaviour
{
	[SerializeField] private GameObject _from;
	[SerializeField] private float _maxZDistance;

	class Baker : Baker<TeleportIfTooFarAuthoring>
	{
		public override void Bake(TeleportIfTooFarAuthoring authoring)
		{
			var teleportIfTooFar = new TeleportIfTooFar {
				From = GetEntity(authoring._from),
                MaxZDistance = authoring._maxZDistance
			};

			AddComponent(teleportIfTooFar);
		}
	}
}