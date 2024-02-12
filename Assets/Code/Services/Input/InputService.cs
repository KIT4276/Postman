using UnityEngine;

public abstract class InputService : IInputService
{
    private const string FireButton = "Fire";
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";

    public abstract Vector2 Axis { get; }

    public bool IsAttackButtonUp()=>
        SimpleInput.GetButtonUp(FireButton);

    protected static Vector2 SimpleInputAxis()=>
        new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
}

