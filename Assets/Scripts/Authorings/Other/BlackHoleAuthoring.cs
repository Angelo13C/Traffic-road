using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class BlackHoleAuthoring : MonoBehaviour
{
	[SerializeField] private PhysicsCategoryTags _becomeStaticOnTouchTags;
    [SerializeField] private float _becomeStaticInitialRadius = 0.5f;
    [SerializeField] private float _becomeStaticRadiusGrowthPerSecond = 0.5f;

	class Baker : Baker<BlackHoleAuthoring>
	{
		public override void Bake(BlackHoleAuthoring authoring)
		{
			var blackHole = new BlackHole {
				LastScale = authoring.transform.localScale.x,
				BecomeStaticOnTouchTags = authoring._becomeStaticOnTouchTags,
				BecomeStaticRadius = authoring._becomeStaticInitialRadius,
				BecomeStaticRadiusGrowthPerSecond = authoring._becomeStaticRadiusGrowthPerSecond
			};

			AddComponent(blackHole);
		}
	}
}