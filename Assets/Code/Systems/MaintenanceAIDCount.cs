using System.Collections.Generic;
using UnityEngine;

public class MaintenanceAIDCount
{
    private const string AIDSpawnerTag = "AIDSpawner";

    private readonly AIDFactory _aidFactory;
    private readonly PersistantStaticData _staticData;

    private readonly List<AIDSpawner> _spawners = new ();

    public MaintenanceAIDCount(AIDFactory aidFactory, PersistantStaticData staticData)
    {
        _aidFactory = aidFactory;
        _staticData = staticData;
        _aidFactory.ChangeAIDCount += CheckAIDCount;
    }

    public void SetSpawners()
    {
        foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(AIDSpawnerTag))
        {
            if (spawnerObject.TryGetComponent<AIDSpawner>(out AIDSpawner spawner))
                _spawners.Add(spawner);
        }
    }

    private void CheckAIDCount()
    {
        if (_aidFactory.Aids.Count < _staticData.AidMinCount) //move magic number to stats  
            foreach (var spawner in _spawners)
                spawner.SpawnIfEmpty();
    }
}
