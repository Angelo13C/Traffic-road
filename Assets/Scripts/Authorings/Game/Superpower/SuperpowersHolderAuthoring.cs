using Unity.Entities;
using UnityEngine;

public class SuperpowersHolderAuthoring : MonoBehaviour
{
	[SerializeField] private SuperpowersHolder _superPowers;

	[SerializeField] private GameObject _rockToThrowPrefab;
	[SerializeField] private GameObject _blackHolePrefab;

	class Baker : Baker<SuperpowersHolderAuthoring>
	{
		public override void Bake(SuperpowersHolderAuthoring authoring)
		{
			authoring._superPowers.ThrowRock.RockPrefab = GetEntity(authoring._rockToThrowPrefab);
			authoring._superPowers.BlackHole.BlackHolePrefab = GetEntity(authoring._blackHolePrefab);
			AddComponent(authoring._superPowers);
			AddComponent(authoring._superPowers.BlackHole);
		}
	}
}