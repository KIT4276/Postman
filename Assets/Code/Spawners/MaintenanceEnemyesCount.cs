using System;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceEnemyesCount  
{
    private const string EnemySpawnerTag = "EnemySpawner";
    private readonly EnemyFactory _factory;

    private List<EnemySpawner> _spawners = new List<EnemySpawner>();

    public MaintenanceEnemyesCount(EnemyFactory enemyFactory)
    {
        _factory = enemyFactory;

        _factory.DeadSumEnemyEvent += CheckEnemiesCount;
    }

    public void SetSpawners()
    {
        if(GameObject.FindGameObjectsWithTag(EnemySpawnerTag) != null)
            foreach (var spawner in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
            {
                _spawners.Add(spawner.GetComponent<EnemySpawner>());
            }

    }

    private void CheckEnemiesCount()
    {
        if (_factory.Enemies.Count <= 6) //move magic number to stats  
            foreach (var spawner in _spawners)
            {
                spawner.SpawnIfEmpty();
            }
    }
}
