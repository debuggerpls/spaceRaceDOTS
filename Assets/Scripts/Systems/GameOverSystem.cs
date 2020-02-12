using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class GameOverSystem : JobComponentSystem
{
    private EntityQuery playerQuery;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        playerQuery = GetEntityQuery(typeof(PlayerData), typeof(PlayerMovementData));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float yPos = -GameManager.main.yBound + 1;
        
        Entities
            .WithAll<GameOverTag>()
            .ForEach((Entity entity) =>
            {
                // move players to start position and disable inputs
                NativeArray<Entity> entities = playerQuery.ToEntityArray(Allocator.TempJob);
                for (int i = 0; i < entities.Length; i++)
                {
                    float3 pos = EntityManager.GetComponentData<Translation>(entities[i]).Value;
                    pos.y = yPos;
                    EntityManager.SetComponentData(entities[i], new Translation(){ Value = pos });
                    EntityManager.AddComponentData(entities[i], new ImmobileTag());
                }
                entities.Dispose();
                
                // create restart entity so its waiting for restart
                EntityManager.CreateEntity(typeof(WaitForRestartTag));
                // delete itself
                EntityManager.DestroyEntity(entity);

            }).WithStructuralChanges().Run();

        return default;
    }
}
