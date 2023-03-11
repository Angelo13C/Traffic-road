using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MapObstaclesAuthoring : MonoBehaviour
{
    [System.Serializable]
    private struct Obstacle
    {
        public GameObject Prefab;
        public int Weight;
    }

    [Header("Grass")]
    [SerializeField] private int2 _grassObstaclesCountRange = new int2(10, 60);
    [SerializeField] private Obstacle[] _grassStaticObstacles = new Obstacle[1];

    [Header("Road")]
    [SerializeField] private float2 _distanceRangeBetweenObstacles = new float2(5, 20);
    [SerializeField] private Obstacle[] _roadDynamicObstacles = new Obstacle[1];

    [Header("Water")]
    [SerializeField] private Obstacle[] _waterStaticObstacles = new Obstacle[1];
    [SerializeField] private Obstacle[] _waterDynamicObstacles = new Obstacle[1];

	class Baker : Baker<MapObstaclesAuthoring>
	{
		public override void Bake(MapObstaclesAuthoring authoring)
		{
            AddObstaclesBuffer<GrassStaticObstacles>(authoring._grassStaticObstacles, out var totalWeight);
            AddComponent(new GrassObstaclesConfig {
                ObstaclesCount = authoring._grassObstaclesCountRange,
                TotalWeight = totalWeight
            });

            AddObstaclesBuffer<RoadDynamicObstacles>(authoring._roadDynamicObstacles, out totalWeight);
            AddComponent(new RoadObstaclesConfig {
                TotalWeight = totalWeight,
                DistanceRangeBetweenObstacles = authoring._distanceRangeBetweenObstacles
            });

            AddObstaclesBuffer<WaterStaticObstacles>(authoring._waterStaticObstacles, out totalWeight);
            AddObstaclesBuffer<WaterDynamicObstacles>(authoring._waterDynamicObstacles, out totalWeight);
        }

        private void AddObstaclesBuffer<T>(Obstacle[] obstacles, out int totalWeight)
            where T : unmanaged, ObstaclesBuffer
        {
            totalWeight = 0;
            var buffer = AddBuffer<T>();
            buffer.ResizeUninitialized(obstacles.Length);
            for(var i = 0; i < obstacles.Length; i++)
            {
                buffer[i] = new T {
                    Prefab = GetEntity(obstacles[i].Prefab),
                    Weight = obstacles[i].Weight
                };
                totalWeight += obstacles[i].Weight;
            }
        }
	}
}