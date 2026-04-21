using UnityEngine;

public interface IInputService : IService
{
    bool IsEnabled { get; set; }

    Vector2 Axis { get; }

    bool IsAttackButtonUp();
}
