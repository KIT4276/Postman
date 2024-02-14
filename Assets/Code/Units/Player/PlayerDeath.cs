using UnityEngine;

public class PlayerDeath : Death
{
    public PlayerHealth Health;
    public PlayerMove Move;
    public PlayerAttack Attack;
    public PlayerAnimator Animator;
    public GameObject DeathFX;

    private bool _isDead;

    private void Start() =>
        Health.HealthChanged += HealthChanged;

    private void OnDestroy() =>
        Health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if (!_isDead && Health.Current <= 0)
            Die();
    }

    private void Die()
    {
        _isDead = true;

        Move.enabled = false;
        Attack.enabled = false;
        Animator.PlayDeath();

        Instantiate(DeathFX, transform.position, Quaternion.identity);
    }

    protected override void OnDead()
    {
        //todo
    }
}
