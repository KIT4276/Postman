using System;
using UnityEngine;


[RequireComponent(typeof(UniqId))]
public class AddressTrigger : BaseTrigger
{
    [SerializeField] protected UniqId _uniqId;

    public string Id { get => _uniqId.Id; }

    public event Action<string> AddressTriggerEnter;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            AddressTriggerEnter?.Invoke(Id);

            this.gameObject.SetActive(false);
        }
    }
}

