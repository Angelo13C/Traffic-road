using Unity.Entities;
using Unity.Mathematics;

public interface ObstaclesBuffer : IBufferElementData
{
    public Entity Prefab { get; set; }
    public int Weight { get; set; }
}

public interface ObstaclesConfig : IComponentData
{
    public int2 ObstaclesCount { get; set; }
    public int TotalWeight { get; set; }
}

public struct GrassObstaclesConfig : IComponentData, ObstaclesConfig
{
    public int2 ObstaclesCount { get; set; }
    public int TotalWeight { get; set; }
}

public struct RoadObstaclesConfig : IComponentData
{
    public int TotalWeight;
    public float2 DistanceRangeBetweenObstacles;
}

public struct WaterObstaclesConfig : IComponentData
{
    public int TotalWeight;
    public float2 DistanceRangeBetweenObstacles;
}

[InternalBufferCapacity(0)]
public struct GrassStaticObstacles : IBufferElementData, ObstaclesBuffer
{
    public Entity Prefab { get; set; }
    public int Weight { get; set; }
}

[InternalBufferCapacity(0)]
public struct RoadDynamicObstacles : IBufferElementData, ObstaclesBuffer
{
    public Entity Prefab { get; set; }
    public int Weight { get; set; }
}

[InternalBufferCapacity(0)]
public struct WaterStaticObstacles : IBufferElementData, ObstaclesBuffer
{
    public Entity Prefab { get; set; }
    public int Weight { get; set; }
}

[InternalBufferCapacity(0)]
public struct WaterDynamicObstacles : IBufferElementData, ObstaclesBuffer
{
    public Entity Prefab { get; set; }
    public int Weight { get; set; }
}