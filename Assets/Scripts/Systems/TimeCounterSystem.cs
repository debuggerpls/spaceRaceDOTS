using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class TimeCounterSystem : JobComponentSystem
{
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((Entity entity, ref TimeCounterData data) =>
        {
            if (!GameManager.main.gameOver)
            {
                float time = data.TimeInSeconds - deltaTime;
                data.TimeInSeconds = time;
                GameManager.main.UpdateTime(time);

                // game over
                if (data.TimeInSeconds <= 0)
                {
                    // notify gamemanager
                    GameManager.main.TimeIsOut();
                    // create gameover entity
                    Entity ent = EntityManager.CreateEntity();
                    EntityManager.AddComponentData(ent, new GameOverTag());
                    // delete itself
                    EntityManager.DestroyEntity(entity);
                }
            }
        }).WithoutBurst().WithStructuralChanges().Run();

        return default;
    }
}
