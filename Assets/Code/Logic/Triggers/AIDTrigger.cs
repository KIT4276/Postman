using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class AIDTrigger : BaseTrigger
{
    public class Pool : MonoMemoryPool<AIDTrigger> { }
    private float _health;

    [Inject]
    private AIDFactory _factory;

    public event Action OffAID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHealth>(out IHealth health))
            health.ChangeHealth(_health);

        OffAID?.Invoke();
        _factory.DespawnAID(this);
    }

    public void SetRecoveryHealth(float value) => 
        _health = value;
}


