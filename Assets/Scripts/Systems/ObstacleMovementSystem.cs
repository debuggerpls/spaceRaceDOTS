using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ObstacleMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        
        JobHandle myJob = Entities.ForEach((ref Translation pos, in ObstacleMovementData data) =>
        {
            float2 position = pos.Value.xy;
            position.x += data.Direction * data.Speed * deltaTime;
            pos.Value.xy = position;
        }).Schedule(inputDeps);

        return myJob;
    }
}
