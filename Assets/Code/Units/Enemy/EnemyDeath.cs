using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyDeath : Death
{
    [SerializeField]
    private EnemyHealth Health;
    [SerializeField]
    private EnemyAnimator Animator;
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private GameObject DeathFX;
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
        //SpawnDwathFX();
        Agent.enabled = false;
        //StartCoroutine(DestroyTimer());
    }

    protected override void OnDead()
    {
        Happened?.Invoke();
        Instantiate(DestroyFX, transform.position, Quaternion.identity);

        
        _factory.DespawnEnemy(_enemy);
    }

    //private void SpawnDwathFX() =>
    //    Instantiate(DeathFX, transform.position, Quaternion.identity);

    //private IEnumerator DestroyTimer()
    //{
    //    yield return new WaitForSeconds(DestroyDelay);

    //    Instantiate(DestroyFX, transform.position, Quaternion.identity);

    //    Happened?.Invoke();
    //    _factory.DespawnEnemy(_enemy);
    //}
}
