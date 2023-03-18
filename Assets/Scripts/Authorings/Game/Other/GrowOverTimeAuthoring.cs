using Unity.Entities;
using UnityEngine;

public class GrowOverTimeAuthoring : MonoBehaviour
{
	[SerializeField] private float _growthPerSecond = 0.5f;

	class Baker : Baker<GrowOverTimeAuthoring>
	{
		public override void Bake(GrowOverTimeAuthoring authoring)
		{
			var growOverTime = new GrowOverTime {
				GrowthPerSecond = authoring._growthPerSecond
			};

			AddComponent(growOverTime);
		}
	}
}