using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataScriptableObject", order = 1)]
public class PersistantPlayerStaticData : ScriptableObject
{
    [SerializeField] private float _maxHP;
    [SerializeField] private float _damage;
    [SerializeField] private float _damageRadius;
    [SerializeField] private float _infectionSpeed;

    public float MaxHP => _maxHP;
    public float Damage => _damage;
    public float DamageRadius => _damageRadius;

    public float InfectionSpeed => _infectionSpeed;
}
