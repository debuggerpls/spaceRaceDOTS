using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class ObstacleSpawnSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        float xBound = GameManager.main.xBound;
        float upper = GameManager.main.yUpperBoundObstacle;
        float lower = GameManager.main.yLowerBoundObstacle;
        float chance = GameManager.main.chanceToSpawn;
        float minTimeout = GameManager.main.minSpawnTimeout;
        float maxTimeout = GameManager.main.maxSpawnTimeout;
        float minSpeed = GameManager.main.minObstacleSpeed;
        float maxSpeed = GameManager.main.maxObstacleSpeed;
        Entity prefab = GameManager.main.obstacleEntityPrefab;
        
        Entities.ForEach((ref SpawnTimeoutData data) =>
        {
            if (data.TimeoutInSeconds <= 0 && !GameManager.main.gameOver)
            {
                Random rnd = new Random((uint) math.floor(deltaTime * 12345));
                int direction;
                float speed;
                // spawn shit
                for (float y = lower; y <= upper; y += 0.5f)
                {
                    if (rnd.NextFloat() <= chance)
                    {
                        direction = rnd.NextFloat() < 0.5f ? 1 : -1;
                        speed = rnd.NextFloat(minSpeed, maxSpeed);
                        Entity newObst = ecb.Instantiate(prefab);
                        ecb.SetComponent(newObst, new Translation
                        {
                            Value = new float3(direction > 0 ? -xBound : xBound, y, 0f)
                        });
                        ecb.SetComponent(newObst, new ObstacleMovementData
                        {
                            Direction = direction,
                            Speed = rnd.NextFloat(minSpeed,maxSpeed)
                        });
                    }
                }
                // set new timeout
                data.TimeoutInSeconds = rnd.NextFloat(minTimeout, maxTimeout);
            }
            else
            {
                data.TimeoutInSeconds -= deltaTime;
            }
        }).WithoutBurst().Run();
    
        ecb.Playback(EntityManager);
        ecb.Dispose();
        
        return default;
    }
}
