using Unity.Entities;
using UnityEngine;

public class MapViewerAuthoring : MonoBehaviour
{
	[SerializeField] [Min(1)] private int _visibleTilesCount = 5;
	
	class Baker : Baker<MapViewerAuthoring>
	{
		public override void Bake(MapViewerAuthoring authoring)
		{
			var mapViewer = new MapViewer {
				VisibleTilesCount = authoring._visibleTilesCount
			};
			AddComponent(mapViewer);
		}
	}
}