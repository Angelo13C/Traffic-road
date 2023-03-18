using Unity.Entities;
using UnityEngine;

public class LookAtAuthoring : MonoBehaviour
{
	[SerializeField] private GameObject _objectToLook;

	class Baker : Baker<LookAtAuthoring>
	{
		public override void Bake(LookAtAuthoring authoring)
		{
			var lookAt = new LookAt {
				EntityToLook = GetEntity(authoring._objectToLook)
			};

			AddComponent(lookAt);
		}
	}
}