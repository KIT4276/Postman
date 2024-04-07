using System;
using UnityEngine;
using Zenject;

public class SaveTrigger : BaseTrigger
{
    [Inject] private readonly ISaveLoadService _saveLoadService;

    public event Action PlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            _saveLoadService.SaveProgress();
            PlayerEnter?.Invoke();
        }
    }
}
