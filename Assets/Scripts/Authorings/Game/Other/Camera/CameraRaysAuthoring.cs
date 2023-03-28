using Unity.Entities;
using UnityEngine;

public class CameraRayAuthoring : MonoBehaviour
{
	class Baker : Baker<CameraRayAuthoring>
	{
		public override void Bake(CameraRayAuthoring authoring)
		{
			var cameraReference = new CameraReference
			{
				Camera = Camera.main
			};
			AddComponentObject(cameraReference);
			
			var cameraRays = new CameraRays {
				Mouse = cameraReference.Camera.ScreenPointToRay(Input.mousePosition),
				ScreenCenter =  cameraReference.Camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2))
			};
			AddComponent(cameraRays);
		}
	}
}