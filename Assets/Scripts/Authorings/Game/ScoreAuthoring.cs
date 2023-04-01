using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreAuthoring : MonoBehaviour
{
	[SerializeField] private bool _displayInUI;

	class Baker : Baker<ScoreAuthoring>
	{
		public override void Bake(ScoreAuthoring authoring)
		{
			var score = new Score {
				Current = 0,
				DisplayInUI = authoring._displayInUI
			};
			AddComponent(score);

			var scoreOnTravel = new ScoreOnTravel {
				HighestTileIndex = 0
			};
			AddComponent(scoreOnTravel);
		}
	}
}