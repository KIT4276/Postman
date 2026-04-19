using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerHealth : MonoBehaviour, IHealth, ISavedProgress
{
    [SerializeField]
    private PlayerAnimator _animator;

    private State _state = new();

    public event Action HealthChanged;
    public event Action GetHit;

    public float Current
    {
        get => State.CurrentHP;
        set
        {
            if (State.CurrentHP != value)
            {
                State.CurrentHP = value;
                HealthChanged?.Invoke();
            }
        }
    }
    public float Max
    {
        get => State.MaxHP;
        set => State.MaxHP = value;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _state = progress.PlayerState ?? new State();
        progress.PlayerState = _state;
        HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.PlayerState.CurrentHP = Current;
        progress.PlayerState.MaxHP = Max;
    }

    public void ChangeHealth(float health)
    {
        if (Current <= 0)
            return;
        if (Current >= Max)
            Current = Max;

        Current += health;

        if (health < 0)
        {
            _animator.PlayHit();
            GetHit?.Invoke();
        }

    }

    private State State => _state ??= new State();
}
