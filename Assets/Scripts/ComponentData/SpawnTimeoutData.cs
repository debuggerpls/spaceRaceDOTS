using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpawnTimeoutData : IComponentData
{
    public float TimeoutInSeconds;
}
