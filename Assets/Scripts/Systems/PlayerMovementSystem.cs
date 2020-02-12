using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float yBound = GameManager.main.yBound;
        
        Entities
            .WithNone<ImmobileTag>()
            .ForEach((ref Translation pos, in PlayerMovementData data) =>
        {
            float2 position = pos.Value.xy;
            position.y = math.clamp(position.y + data.Direction * data.Speed * deltaTime, -yBound, yBound);
            pos.Value.xy = position;
        }).Run();

        return default;
    }
}
