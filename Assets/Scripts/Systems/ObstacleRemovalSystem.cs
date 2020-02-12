using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ObstacleRemovalSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float xBound = GameManager.main.xBound;
        EntityCommandBuffer.Concurrent ecb = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
        JobHandle myJob = Entities
            .WithAll<ObstacleMovementData>()
            .ForEach((Entity entity, int entityInQueryIndex, in Translation pos) =>
            {
                if (pos.Value.x > xBound || pos.Value.x < -xBound)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
            }).Schedule(inputDeps);
        
        commandBufferSystem.AddJobHandleForProducer(myJob);

        return myJob;
    }
}
