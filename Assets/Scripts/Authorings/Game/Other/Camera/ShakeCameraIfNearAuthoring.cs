using CameraShake;
using Unity.Entities;
using UnityEngine;

public class ShakeCameraIfNearAuthoring : MonoBehaviour
{
	[Tooltip("Radius in which shake doesn't lose strength.")]
	[SerializeField] private float _clippingDistance = 6;
	[Tooltip("How fast strength falls with distance.")]
	[SerializeField] private float _falloffScale = 30;
	[Tooltip("Power of the falloff function.")]
	[SerializeField] private Degree _falloffDegree = Degree.Cubic;
	
	[SerializeField] [Min(0f)] private float _shakeDuration = 0.2f;
	[SerializeField] [Min(0.1f)] private float _shakeForce = 1f;
	[SerializeField] private bool _singleShake = false;

	class Baker : Baker<ShakeCameraIfNearAuthoring>
	{
		public override void Bake(ShakeCameraIfNearAuthoring authoring)
		{
			var shakeCameraIfNear = new ShakeCameraIfNear
			{
				ClippingDistance = authoring._clippingDistance,
				FalloffScale = authoring._falloffScale,
				FalloffDegree = authoring._falloffDegree,
				
				ShakeForce = authoring._shakeForce,
				ShakeDuration = authoring._shakeDuration,
				SingleShake = authoring._singleShake,
			};
			
			AddComponent(shakeCameraIfNear);
		}
	}
}