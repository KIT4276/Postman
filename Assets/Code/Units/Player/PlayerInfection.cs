using System.Collections;
using UnityEngine;

public class PlayerInfection : MonoBehaviour, ISavedProgress
{
    [SerializeField]
    private PlayerHealth _health;

    private bool _heal;
    private float _infectionSpeed;

    public float InfectedValue { get; private set; }
    public bool IsInfectes { get; private set; }

    public void Init(PersistantPlayerStaticData playerData) =>
        _infectionSpeed = playerData.InfectionSpeed;

    private void Start() => 
        _health.GetHit += StartInfection;

    public void StopInfection()
    {
        _heal = true;

        IsInfectes = false;
        InfectedValue = 0;
    }

    private void StartInfection()
    {
        IsInfectes = true;
        _heal = false;
        StartCoroutine(InfectionCoroutine());
    }

    private IEnumerator InfectionCoroutine()
    {
        while (InfectedValue < 100)
        {
            InfectedValue += _infectionSpeed * Time.deltaTime;
            if (_heal) yield break;

            yield return null;
        }
        if (!_heal)
            _health.ChangeHealth(-_health.Current);
    }


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

    private void OnDestroy() => 
        _health.GetHit -= StartInfection;
}
