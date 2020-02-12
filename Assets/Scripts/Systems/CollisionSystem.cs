using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

public class CollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    
    [BurstCompile]
    private struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<Translation> translationEntities;
        public ComponentDataFromEntity<PlayerData> playerEntities;
        public float yPos;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            if (playerEntities.HasComponent(triggerEvent.Entities.EntityA))
            {
                Translation translation = translationEntities[triggerEvent.Entities.EntityA];
                translation.Value.y = yPos;
                translationEntities[triggerEvent.Entities.EntityA] = translation;
            }
            
            if (playerEntities.HasComponent(triggerEvent.Entities.EntityB))
            {
                Translation translation = translationEntities[triggerEvent.Entities.EntityB];
                translation.Value.y = yPos;
                translationEntities[triggerEvent.Entities.EntityB] = translation;
            }
        }
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float yPos = -GameManager.main.yBound + 2;
        TriggerJob triggerJob = new TriggerJob
        {
            yPos = yPos,
            playerEntities = GetComponentDataFromEntity<PlayerData>(),
            translationEntities = GetComponentDataFromEntity<Translation>()
        };
        return triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    }
}
