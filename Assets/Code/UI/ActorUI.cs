using UnityEngine;

public partial class ActorUI : MonoBehaviour
{
    [SerializeField] private Bar HpBar;

    private IHealth _heroHealth;

    public void Construct(IHealth health)
    {
        _heroHealth = health;

        _heroHealth.HealthChanged += UpdateHpBar;
    }

    private void UpdateHpBar() =>
        HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);

    private void OnDestroy()
    {
        _heroHealth.HealthChanged -= UpdateHpBar;
    }
}
