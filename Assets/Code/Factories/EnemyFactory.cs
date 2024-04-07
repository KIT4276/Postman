using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : IGameplayFactory
{
    [Inject]
    private readonly Enemy.Pool _enemiesPool;
    [Inject]
    private readonly EntenemiesParent _parent;

    public List<Enemy> Enemies { get; private set; }  = new List<Enemy>();

    public event Action ChangeEnemiesCount;
    public event Action DeadSumEnemyEvent;

    public Enemy SpawnEnemy(Transform spawnerTransform)
    {
        Enemy enemy = _enemiesPool.Spawn();
        enemy.NavMeshEnabled(true);
        Enemies.Add(enemy);
        enemy.transform.parent = _parent.transform;
        
        enemy.transform.SetPositionAndRotation(spawnerTransform.position, spawnerTransform.rotation);

        ChangeEnemiesCount?.Invoke();
        enemy.Restart();
        enemy.GetComponent<EnemyDeath>().Happened += DeadEnemy;
        return enemy;
    }

    public void DespawnEnemy(Enemy enemy)
    {
        _enemiesPool.Despawn(enemy);
        Enemies.Remove(enemy);
        enemy.NavMeshEnabled(false);
        ChangeEnemiesCount?.Invoke();
    }

    public int GetEnemiesCount()
        => Enemies.Count;

    private void DeadEnemy()
        => DeadSumEnemyEvent?.Invoke();
}
