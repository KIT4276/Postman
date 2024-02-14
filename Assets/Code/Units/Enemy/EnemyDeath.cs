using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyDeath : Death
{
    [SerializeField]
    private EnemyHealth Health;
    [SerializeField]
    private EnemyAnimator Animator;
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private float DestroyDelay = 3;
    [SerializeField]
    private GameObject DestroyFX;
    [SerializeField]
    private Enemy _enemy;

    [Inject]
    private EnemyFactory _factory;

    public event Action Happened;

    private void Start() =>
        Health.HealthChanged += HealthChanged;

    public void Restart() =>
        Health.HealthChanged += HealthChanged;

    private void OnDestroy() =>
        Health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if (Health.Current <= 0)
            Die();
    }

    private void Die()
    {
        Health.HealthChanged -= HealthChanged;

        Animator.PlayDeath();
        Agent.enabled = false;
    }

    protected override void OnDead()
    {
        Happened?.Invoke();
        Instantiate(DestroyFX, transform.position, Quaternion.identity);
        
        _factory.DespawnEnemy(_enemy);
    }
}
