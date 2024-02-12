using System;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField]
    private EnemyAnimator EnemyAnimator;
    [SerializeField]
    private float _current;
    [SerializeField]
    private float _max;

    public float Current
    {
        get => _current;
        set => _current = value;
    }
    public float Max
    {
        get => _max;
        set => _max = value;
    }

    public void Restart() =>
        Current = Max;

    public event Action HealthChanged;

    public void ChangeHealth(float health)
    {
        Current += health;

        EnemyAnimator.PlayHit();

        HealthChanged?.Invoke();
    }
}
