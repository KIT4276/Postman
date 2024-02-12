using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerHealth :MonoBehaviour, IHealth, ISavedProgress
{
    public PlayerAnimator Animator;

    private State _state;

    public event Action HealthChanged;

    public float Current
    {
        get => _state.CurrentHP;
        set
        {
            if (_state.CurrentHP != value)
            {
                _state.CurrentHP = value;
                HealthChanged?.Invoke();
            }
        }
    }
    public float Max
    {
        get => _state.MaxHP;
        set => _state.MaxHP = value;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _state = progress.HeroState;
        HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.HeroState.CurrentHP = Current;
        progress.HeroState.MaxHP = Max;
    }

    public void ChangeHealth(float health)
    {
        if (Current <= 0)
            return;
        if (Current >= Max)
            Current = Max;

        Current += health;
        Animator.PlayHit();
    }
}