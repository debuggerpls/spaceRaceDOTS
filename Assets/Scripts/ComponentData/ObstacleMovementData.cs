using Unity.Entities;

[GenerateAuthoringComponent]
public struct ObstacleMovementData : IComponentData
{
    public int Direction;
    public float Speed;
}
