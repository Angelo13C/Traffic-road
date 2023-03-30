using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
[BurstCompile]
public partial struct NearbyCarsTrackerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (nearbyCarsTracker, nearbyCars, trackerTransform) in SystemAPI.Query<NearbyCarsTracker, DynamicBuffer<NearbyCars>, LocalTransform>())
        {
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            var hits = new NativeArray<DistanceHit>(nearbyCarsTracker.MaxTrackedCarsCount, Allocator.Temp,
                NativeArrayOptions.UninitializedMemory);
            var hitsCollector = new LimitedHitsCollector<DistanceHit>(nearbyCarsTracker.Radius, ref hits);
            if (physicsWorld.OverlapSphereCustom(trackerTransform.Position, nearbyCarsTracker.Radius, ref hitsCollector, nearbyCarsTracker.CollisionFilter))
            {
                nearbyCars.ResizeUninitialized(hitsCollector.NumHits);
                for (var i = 0; i < hitsCollector.NumHits; i++)
                {
                    nearbyCars.ElementAt(i) = new NearbyCars
                    {
                        Entity = hits[i].Entity,
                        Position = hits[i].Position
                    };
                }
            }
            else
                nearbyCars.ResizeUninitialized(0);
        }
    }
    
    private struct LimitedHitsCollector<T> : ICollector<T> where T : unmanaged, IQueryResult
    {
        public float MaxFraction { get; }

        public bool EarlyOutOnFirstHit => AllHits.Length <= 1;

        public int NumHits { get; private set; }

        public NativeArray<T> AllHits;

        public LimitedHitsCollector(float maxFraction, ref NativeArray<T> allHits)
        {
            MaxFraction = maxFraction;
            AllHits = allHits;
            NumHits = 0;
        }

        public bool AddHit(T hit)
        {
            if (NumHits >= AllHits.Length)
                return false;
            
            AllHits[NumHits] = hit;
            NumHits++;
            return true;
        }
    }
}