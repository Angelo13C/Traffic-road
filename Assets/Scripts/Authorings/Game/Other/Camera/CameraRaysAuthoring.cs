using Unity.Entities;
using UnityEngine;

public class CameraRayAuthoring : MonoBehaviour
{
	class Baker : Baker<CameraRayAuthoring>
	{
		public override void Bake(CameraRayAuthoring authoring)
		{
			AddComponent<CameraRays>();
		}
	}
}