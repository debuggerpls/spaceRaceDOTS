using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerMovementData : IComponentData
{
    public int Direction;
    public float Speed;
}
