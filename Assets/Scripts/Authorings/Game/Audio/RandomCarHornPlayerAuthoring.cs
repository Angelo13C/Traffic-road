using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomCarHornPlayerAuthoring : MonoBehaviour
{
	[SerializeField] private float2 _timeRangeBetweenHorns = new float2(1.2f, 2.5f);
	[SerializeField] private AudioPlayer _audioPlayer;

	class Baker : Baker<RandomCarHornPlayerAuthoring>
	{
		public override void Bake(RandomCarHornPlayerAuthoring authoring)
		{
			var randomCarHornsPlayer = new RandomCarHornPlayer
			{
				TimeRangeBetweenHorns = authoring._timeRangeBetweenHorns,
				RemainingTimeToHorn = Random.Range(authoring._timeRangeBetweenHorns.x, authoring._timeRangeBetweenHorns.y),
				AudioPlayer = authoring._audioPlayer
			};
			
			AddComponentObject(randomCarHornsPlayer);
		}
	}
}