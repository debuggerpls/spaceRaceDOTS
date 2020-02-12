using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class WaitForRestartSystem : JobComponentSystem
{
    private EntityQuery playerQuery;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        playerQuery = GetEntityQuery(typeof(PlayerData), typeof(PlayerMovementData), typeof(ImmobileTag));
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float time = GameManager.main.newGameTimeInSeconds;
        Entities
            .WithAll<WaitForRestartTag>()
            .ForEach((Entity entity) =>
        {
            if (Input.GetKey(KeyCode.R))
            {
                // turn on playermovementsystem
                NativeArray<Entity> entities = playerQuery.ToEntityArray(Allocator.TempJob);
                for (int i = 0; i < entities.Length; i++)
                {
                    EntityManager.RemoveComponent<ImmobileTag>(entities[i]);
                }
                entities.Dispose();
                // create new time entity
                Entity newTime = EntityManager.CreateEntity();
                EntityManager.AddComponentData(newTime, new TimeCounterData {TimeInSeconds = time});
                // notify gamemanager
                GameManager.main.Restart();
                // delete itself
                EntityManager.DestroyEntity(entity);
            }
        }).WithoutBurst().WithStructuralChanges().Run();

        return default;
    }
}
