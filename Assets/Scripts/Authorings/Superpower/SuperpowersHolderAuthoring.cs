using Unity.Entities;
using UnityEngine;

public class SuperpowersHolderAuthoring : MonoBehaviour
{
	[SerializeField] private SuperpowersHolder _superPowers;

	[SerializeField] private GameObject _rockToThrowPrefab;

	class Baker : Baker<SuperpowersHolderAuthoring>
	{
		public override void Bake(SuperpowersHolderAuthoring authoring)
		{
			authoring._superPowers.ThrowRock.RockPrefab = GetEntity(authoring._rockToThrowPrefab);
			AddComponent(authoring._superPowers);
		}
	}
}