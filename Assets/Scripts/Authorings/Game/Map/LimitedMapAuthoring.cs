using Unity.Entities;
using UnityEngine;

public class LimitedMapAuthoring : MonoBehaviour
{
	[SerializeField] private int _tilesCount = 100;
	class Baker : Baker<LimitedMapAuthoring>
	{
		public override void Bake(LimitedMapAuthoring authoring)
		{
			var limitedMap = new LimitedMap
			{
				TilesCount = authoring._tilesCount
			};
			AddComponent(limitedMap);
		}
	}
}