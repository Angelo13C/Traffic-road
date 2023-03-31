using System;
using Unity.Entities;
using UnityEngine;

public class FootstepAudioPlayerAuthoring : MonoBehaviour
{
	[SerializeField] private AudioPlayer _audioPlayer;
	[SerializeField] private float _footYOffset = -0.8f;

	class Baker : Baker<FootstepAudioPlayerAuthoring>
	{
		public override void Bake(FootstepAudioPlayerAuthoring authoring)
		{
			var footstepAudioPlayer = new FootstepAudioPlayer
			{
				AudioPlayer = authoring._audioPlayer,
				FootYOffset = authoring._footYOffset
			};
			
			AddComponentObject(footstepAudioPlayer);
		}
	}

	private void OnDrawGizmos()
	{
		var footPosition = transform.position;
		footPosition.y += _footYOffset;
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(footPosition, 0.2f);
	}
}