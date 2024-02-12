using UnityEngine;
using Zenject;

public class AIDSpawner : Spawner
{
    [SerializeField]
    private float _healthValue = 10;

    private string _id;
    public bool _empty;
    private AIDTrigger _aid;
    
    [Inject]
    private AIDFactory _factory;

    private void Awake() =>
        _id = GetComponent<UniqId>().Id;

    public void SpawnIfEmpty()
    {
        if (_empty)
            Spawn();
    }

    public override void LoadProgress(PlayerProgress progress)
    {
        if (progress.KillData.CleatedSpawners.Contains(_id))
            _empty = true;
        else
            Spawn();
    }

    private void Spawn()
    {
        _aid  = _factory.SpawnAID(this.transform, _healthValue);
        _aid.OffAID += OffAID;
    }

    public override void UpdateProgress(PlayerProgress progress)
    {
        if (_empty)
            progress.KillData.CleatedSpawners.Add(_id);
    }

    private void OffAID()
    {
        _empty = true;
        _aid.OffAID -= OffAID;
    }
}
