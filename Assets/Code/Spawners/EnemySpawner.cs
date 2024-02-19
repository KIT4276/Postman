using UnityEngine;
using Zenject;

public class EnemySpawner : Spawner
{
    public MonstersTypeId MonstersTypeId;
    private string _id;
    private EnemyDeath _enemyDeath;
    private Enemy _enemy;

    [SerializeField]
    private bool _slain;

    [Inject]
    private EnemyFactory _enemyFactory;

    private void Awake() =>
        _id = GetComponent<UniqId>().Id;

    public override void LoadProgress(PlayerProgress progress)
    {
        if (progress.KillData.CleatedSpawners.Contains(_id))
            _slain = true;
        else
            Spawn();
    }

    public override void UpdateProgress(PlayerProgress progress)
    {
        if (_slain)
            progress.KillData.CleatedSpawners.Add(_id);
    }

    public void SpawnIfEmpty()
    {
        if (_slain)
            Spawn();
    }

    private void Spawn()
    {
        _enemy = _enemyFactory.SpawnEnemy(this.transform);
        _enemyDeath = _enemy.GetComponent<EnemyDeath>();
        _enemyDeath.Happened += DeadEnemy;
        _slain = false;
    }

    private void DeadEnemy()
    {
        _slain = true;
        _enemy = null;
        _enemyDeath.Happened -= DeadEnemy;
    }
}
