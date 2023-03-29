using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(ExplosiveAuthoring))]
public class ExplodeOnCollisionAuthoring : MonoBehaviour
{
	class Baker : Baker<ExplodeOnCollisionAuthoring>
	{
		public override void Bake(ExplodeOnCollisionAuthoring authoring)
		{
			AddComponent<ExplodeOnCollision>();
		}
	}
}