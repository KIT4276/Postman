using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Follow))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyDeath))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(AnimateAlongAgent))]
[RequireComponent(typeof(Aggro))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(CheckAttackRange))]
[RequireComponent(typeof(ActorUI))]
public class Enemy : MonoBehaviour
{
    public class Pool : MonoMemoryPool<Enemy> { }

    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private Follow _follow;
    [SerializeField]
    private EnemyHealth _enemyHealth;
    [SerializeField]
    private HpBar _hpBar;
    [SerializeField]
    private EnemyAnimator _animator;
    [SerializeField]
    private EnemyDeath _enemyDeath;
    [SerializeField]
    private Attack _attack;
    [SerializeField]
    private Aggro _aggro;
    [SerializeField]
    private CheckAttackRange _checkAttackRAnge;

    private void Start() => 
        _enemyDeath.Happened += Death;

    public void NavMeshEnabled(bool value) =>
        _agent.enabled = value;

    public void Restart()
    {
        _enemyDeath.Happened += Death;

        _enemyHealth.Restart();
        _enemyDeath.Restart();
        _hpBar.SetValue(_enemyHealth.Current, _enemyHealth.Max);
        _attack.Restart();
        _aggro.Restart();
        _checkAttackRAnge.Restart();
    }

    private void Death() => 
        _aggro.End();

    private void OnDisable() => 
        _enemyDeath.Happened -= Death;
}
