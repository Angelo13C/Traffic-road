using Unity.Entities;
using UnityEngine;

public class DestroyOnOutOfBoundsAuthoring : MonoBehaviour
{
	class Baker : Baker<DestroyOnOutOfBoundsAuthoring>
	{
		public override void Bake(DestroyOnOutOfBoundsAuthoring authoring)
		{
			AddComponent<DestroyOnOutOfBounds>();
		}
	}
}