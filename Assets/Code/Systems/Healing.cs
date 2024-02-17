using System;

public class Healing
{

    public event Action ToHealE;

    public void ToHeal()
    {
        ToHealE?.Invoke();
    }
}
