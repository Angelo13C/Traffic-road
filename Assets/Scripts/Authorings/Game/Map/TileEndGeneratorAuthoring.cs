using Unity.Entities;
using UnityEngine;

public class TileEndGeneratorAuthoring : MonoBehaviour
{
	[System.Serializable]
	private class TileEndPrefabs
	{
    	public GameObject Start;
    	public GameObject Continuation;
    	public GameObject End;
	}
    [SerializeField] private TileEndPrefabs _roadTileEndPrefabs;
    [SerializeField] private TileEndPrefabs _grassTileEndPrefabs;
    [SerializeField] private TileEndPrefabs _waterTileEndPrefabs;

	class Baker : Baker<TileEndGeneratorAuthoring>
	{
		public override void Bake(TileEndGeneratorAuthoring authoring)
		{
			var tileEndGenerator = new TileEndGenerator {
				RoadTileEndPrefabs = ToEndPrefabs(authoring._roadTileEndPrefabs),
				GrassTileEndPrefabs = ToEndPrefabs(authoring._grassTileEndPrefabs),
				WaterTileEndPrefabs = ToEndPrefabs(authoring._waterTileEndPrefabs)
			};
			AddComponent(tileEndGenerator);
		}

		private TileEndGenerator.EndPrefabs ToEndPrefabs(TileEndPrefabs prefabs)
		{
			return new TileEndGenerator.EndPrefabs {
				Start = prefabs.Start == null ? Entity.Null : GetEntity(prefabs.Start),
				Continuation = prefabs.Continuation == null ? Entity.Null : GetEntity(prefabs.Continuation),
				End = prefabs.End == null ? Entity.Null : GetEntity(prefabs.End),
			};
		}
	}
}