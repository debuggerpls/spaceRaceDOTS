using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class PlayerReachedGoalSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float yBound = GameManager.main.yBound;
        
        Entities.ForEach((Entity entity, ref Translation pos, in PlayerData player) =>
        {
            if (pos.Value.y >= yBound)
            {
                int playerId = player.ID;
                // move to start pos
                pos.Value.y = -yBound + 1;
                // update score
                GameManager.main.PlayerScored(playerId);
            }
        }).WithoutBurst().Run();
        
        return default;
    }
}
