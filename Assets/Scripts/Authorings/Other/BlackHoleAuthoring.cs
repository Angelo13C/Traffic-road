using Unity.Entities;
using UnityEngine;

public class BlackHoleAuthoring : MonoBehaviour
{
	class Baker : Baker<BlackHoleAuthoring>
	{
		public override void Bake(BlackHoleAuthoring authoring)
		{
			var blackHole = new BlackHole {
				LastScale = authoring.transform.localScale.x
			};

			AddComponent(blackHole);
		}
	}
}