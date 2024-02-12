using UnityEngine;

public partial class ActorUI : MonoBehaviour
{
    public HpBar HpBar;

    private IHealth _heroHealth;

    private bool _isSigned; ////убрать, когда будем работтать с фабриками и спавном

    public void Construct(IHealth health)
    {
        _heroHealth = health;

        _heroHealth.HealthChanged += UpdateHpBar;
        _isSigned = true;
    }

    private void Start() //убрать, когда будем работтать с фабриками и спавном
    {
        IHealth health = GetComponent<IHealth>();

        if (health != null)
            Construct(health);
    }

    private void UpdateHpBar() =>
        HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);

    private void OnDestroy()
    {
        if (_isSigned)
            _heroHealth.HealthChanged -= UpdateHpBar;
    }
}
