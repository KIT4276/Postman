using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameDataScriptableObject", order = 2)]

public class PersistantStaticData : ScriptableObject
{
    [SerializeField] private float _salaryAmount;
    [SerializeField] private int _aidMinCount;
    [SerializeField] private int _enemiesMinCount;
    [SerializeField] private float _healingPrice;

    public float SalaryAmount => _salaryAmount;
    public int AidMinCount => _aidMinCount;
    public int EnemiesMinCount => _enemiesMinCount;
    public float HealingPrice => _healingPrice;
}
