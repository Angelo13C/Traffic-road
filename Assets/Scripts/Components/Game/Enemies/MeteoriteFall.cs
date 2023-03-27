using Unity.Entities;

public struct MeteoriteFall : IComponentData
{
    public Entity MeteoritePrefab;
    public float CurrentZ;
    public float ZChangePerSecond;

    public float SpawnHeight;

    public Config BehindConfig;
    public Config AheadConfig;

    public struct Config
    {
        public float TimeBetweenMeteorites;
        public float RemainingTime;
    }
}