using System.Collections;
using UnityEngine;

public class PlayerInfection : MonoBehaviour, ISavedProgress
{
    [SerializeField]
    private PlayerHealth _health;

    public float InfectedValue { get; private set; }
    public bool IsInfectes { get; private set; }

    private void Start() => 
        _health.GetHit += StartInfection;

    public void StopInfection()
    {
        IsInfectes = false;
        InfectedValue = 0;
    }

    private void StartInfection()
    {
        IsInfectes = true;
        StartCoroutine(InfectionCoroutine());
    }

    private IEnumerator InfectionCoroutine()
    {
        while (InfectedValue < 100)
        {
            InfectedValue += Time.deltaTime;
            yield return null;
        }

        _health.ChangeHealth(-_health.Current);
    }

    private void OnDestroy() => 
        _health.GetHit -= StartInfection;

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.InfectedData.InfectedValue = InfectedValue;
        progress.InfectedData.IsInfectes = IsInfectes;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        InfectedValue = progress.InfectedData.InfectedValue;
        IsInfectes = progress.InfectedData.IsInfectes;

        if (IsInfectes)
            StartInfection();
    }
}
