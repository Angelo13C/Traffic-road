using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class AttractBodiesAuthoring : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] private float _force;
	[SerializeField] private PhysicsCategoryTags _attractableBodiesTags;

	class Baker : Baker<AttractBodiesAuthoring>
	{
		public override void Bake(AttractBodiesAuthoring authoring)
		{
			var attractBodies = new AttractBodies {
				Range = authoring._range,
				Force = authoring._force,
				AttractableBodiesTags = authoring._attractableBodiesTags
			};

			AddComponent(attractBodies);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color32(75, 0, 130, 255);
		Gizmos.DrawWireSphere(transform.position, _range);
	}
}