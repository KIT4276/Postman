using UnityEngine;

public class MobileInputService : InputService
{
    public override Vector2 Axis => IsEnabled ? SimpleInputAxis() : Vector2.zero;
}
