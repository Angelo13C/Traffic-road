using Unity.Entities;
using UnityEngine;

public class SuperpowersInputAuthoring : MonoBehaviour
{
	class Baker : Baker<SuperpowersInputAuthoring>
	{
		public override void Bake(SuperpowersInputAuthoring authoring)
		{
			var superpowersInput = new SuperpowersInput {
				Camera = Camera.main,
			};
			AddComponentObject(superpowersInput);
			AddComponent<Superpowers>();
		}
	}
}