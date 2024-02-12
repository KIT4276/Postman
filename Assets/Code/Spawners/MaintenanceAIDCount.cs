using System;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceAIDCount
{
    private const string AIDSpawnerTag = "AIDSpawner";
    private readonly AIDFactory _aidFactory;


    private List<AIDSpawner> _spawners = new List<AIDSpawner>();

    public MaintenanceAIDCount(AIDFactory aidFactory)
    {
        _aidFactory = aidFactory;

        _aidFactory.ChangeAIDCount += CheckAIDCount;
    }

    public void SetSpawners()
    {
        foreach (var spawner in GameObject.FindGameObjectsWithTag(AIDSpawnerTag))
        {
            _spawners.Add(spawner.GetComponent<AIDSpawner>());
        }
    }

    private void CheckAIDCount()
    {
        if (_aidFactory.Aids.Count < 10) //move magic number to stats  
            foreach (var spawner in _spawners)
            {
                spawner.SpawnIfEmpty();
            }
    }
}
