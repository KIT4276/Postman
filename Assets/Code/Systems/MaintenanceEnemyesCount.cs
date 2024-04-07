using System.Collections.Generic;
using UnityEngine;

public class MaintenanceEnemyesCount
{
    private const string EnemySpawnerTag = "EnemySpawner";

    private readonly EnemyFactory _factory;
    private readonly PersistantStaticData _staticData;

    private readonly List<EnemySpawner> _spawners = new();

    public MaintenanceEnemyesCount(EnemyFactory enemyFactory, PersistantStaticData staticData)
    {
        _factory = enemyFactory;
        _staticData = staticData;
        _factory.DeadSumEnemyEvent += CheckEnemiesCount;
    }

    public void SetSpawners()
    {
        if (GameObject.FindGameObjectsWithTag(EnemySpawnerTag) != null)
            foreach (var spawner in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
                _spawners.Add(spawner.GetComponent<EnemySpawner>());
    }

    private void CheckEnemiesCount()
    {
        if (_factory.Enemies.Count <= _staticData.EnemiesMinCount) //move magic number to stats  
            foreach (var spawner in _spawners)
                spawner.SpawnIfEmpty();
    }
}
