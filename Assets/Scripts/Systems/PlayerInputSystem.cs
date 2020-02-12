using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((ref PlayerMovementData move, in PlayerInputData input) =>
        {
            move.Direction = 0;
            move.Direction += Input.GetKey(input.upKey) ? 1 : 0;
            move.Direction -= Input.GetKey(input.downKey) ? 1 : 0;
        }).Run();

        return default;
    }
}
