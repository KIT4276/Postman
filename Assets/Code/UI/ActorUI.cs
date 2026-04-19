using UnityEngine;

public partial class ActorUI : MonoBehaviour
{
    [SerializeField] private Bar HpBar;

    private IHealth _heroHealth;

    private void Awake()
    {
        if (HpBar == null)
            HpBar = FindChildComponent<Bar>("HP_bar");
    }

    public void Construct(IHealth health)
    {
        if (_heroHealth != null)
            _heroHealth.HealthChanged -= UpdateHpBar;

        _heroHealth = health;

        if (_heroHealth == null)
            return;

        _heroHealth.HealthChanged += UpdateHpBar;
        UpdateHpBar();
    }

    private void UpdateHpBar() =>
        HpBar?.SetValue(_heroHealth?.Current ?? 0, _heroHealth?.Max ?? 0);

    private void OnDisable()
    {
        if (_heroHealth != null)
            _heroHealth.HealthChanged -= UpdateHpBar;
    }

    private void OnDestroy()
    {
        if (_heroHealth != null)
            _heroHealth.HealthChanged -= UpdateHpBar;
    }

    private T FindChildComponent<T>(string childName) where T : Component
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
            if (child.name == childName && child.TryGetComponent(out T component))
                return component;

        return null;
    }
}
