using UnityEngine;
using Zenject;

public partial class ActorUI : MonoBehaviour
{
    [SerializeField]
    private Bar HpBar;

    private IHealth _heroHealth;

    private bool _isSigned; ////remove it when we work with factories and spawn

    public void Construct(IHealth health)
    {
        _heroHealth = health;

        _heroHealth.HealthChanged += UpdateHpBar;
        _isSigned = true;
    }

    //private void Start() //remove it when we work with factories and spawn
    //{
    //    IHealth health = GetComponent<IHealth>();

    //    if (health != null)
    //        Construct(health);
    //}

    private void UpdateHpBar() => 
        HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);

    private void OnDestroy()
    {
        if (_isSigned)
            _heroHealth.HealthChanged -= UpdateHpBar;
    }
}
