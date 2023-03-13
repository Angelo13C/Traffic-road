using Unity.Entities;
using UnityEngine;

public class SuperpowersHolderAuthoring : MonoBehaviour
{
	[SerializeField] private SuperpowersHolder _superPowers;

	class Baker : Baker<SuperpowersHolderAuthoring>
	{
		public override void Bake(SuperpowersHolderAuthoring authoring)
		{
			AddComponent(authoring._superPowers);
		}
	}
}