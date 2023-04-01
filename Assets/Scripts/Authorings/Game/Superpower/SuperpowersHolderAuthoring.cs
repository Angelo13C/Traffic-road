using Unity.Entities;
using UnityEngine;

public class SuperpowersHolderAuthoring : MonoBehaviour
{
	[SerializeField] private GameObject[] _initiallyHoldSuperpowers;
	[SerializeField] private bool _showCurrentSuperpowerInUI;
	
	class Baker : Baker<SuperpowersHolderAuthoring>
	{
		public override void Bake(SuperpowersHolderAuthoring authoring)
		{
			var superpowersUser = new SuperpowerUser
			{
				LastUsedSuperpower = Entity.Null,
				DisplayInUI = authoring._showCurrentSuperpowerInUI
			};
			AddComponent(superpowersUser);
			
			var heldSuperpowers = AddBuffer<HeldSuperpower>();
			heldSuperpowers.ResizeUninitialized(authoring._initiallyHoldSuperpowers.Length);
			for (var i = 0; i < heldSuperpowers.Length; i++)
			{
				heldSuperpowers[i] = new HeldSuperpower
				{
					Prefab = GetEntity(authoring._initiallyHoldSuperpowers[i])
				};
			}
		}
	}
}