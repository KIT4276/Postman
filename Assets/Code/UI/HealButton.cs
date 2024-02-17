using UnityEngine;

public class HealButton : MonoBehaviour
{
    [SerializeField]
    private float _healingPrice = 100; //remove to stats

    private Salary _salary;
    private Healing _healing;

    public void Init(Salary salary, Healing healing)
    {
        _salary = salary;
        _healing = healing;
    }
    
    public void ToHeal()
    {
        _salary.ToSpand(_healingPrice);
        _healing.ToHeal();
    }
}
