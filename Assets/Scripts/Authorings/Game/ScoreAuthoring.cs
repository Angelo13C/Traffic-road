using Unity.Entities;
using UnityEngine;

public class ScoreAuthoring : MonoBehaviour
{
	class Baker : Baker<ScoreAuthoring>
	{
		public override void Bake(ScoreAuthoring authoring)
		{
			var score = new Score {
				Current = 0
			};
			AddComponent(score);

			var scoreOnTravel = new ScoreOnTravel {
				HighestTileIndex = 0
			};
			AddComponent(scoreOnTravel);
		}
	}
}