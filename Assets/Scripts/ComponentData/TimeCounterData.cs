using Unity.Entities;

[GenerateAuthoringComponent]
public struct TimeCounterData : IComponentData
{
    public float TimeInSeconds;
}
