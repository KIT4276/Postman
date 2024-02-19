using UnityEngine;

public class HealButton : MonoBehaviour
{
    [SerializeField]
    private float _healingPrice;// = 100; //remove to stats

    private Salary _salary;
    private Healing _healing;
    private PersistantStaticData _staticData;

    public void Init(Salary salary, Healing healing, PersistantStaticData staticData)
    {
        _salary = salary;
        _healing = healing;
        _staticData = staticData;

        _healingPrice = _staticData.HealingPrice;
    }
    
    public void ToHeal()
    {
        _salary.ToSpand(_healingPrice);
        _healing.ToHeal();
    }
}
