using Unity.Entities;
using UnityEngine;

public class NearbyCarsEnginePlayerAuthoring : MonoBehaviour
{
	[SerializeField] private AudioPlayer _audioPlayer;

	class Baker : Baker<NearbyCarsEnginePlayerAuthoring>
	{
		public override void Bake(NearbyCarsEnginePlayerAuthoring authoring)
		{
			var nearbyCarsEnginePlayer = new NearbyCarsEnginePlayer
			{
				AudioPlayer = authoring._audioPlayer
			};
			
			AddComponentObject(nearbyCarsEnginePlayer);
		}
	}
}