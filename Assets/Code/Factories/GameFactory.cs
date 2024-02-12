using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFactory : IService
{
    [Inject]
    private IAssets _assets;

    public event Action PlayerCreated;

    public GameObject PlayerGameObject { get; private set; }

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private readonly EnemyFactory _enemyFactory;
    private readonly DeliveredParcelsCounter _counter;

    public GameFactory(EnemyFactory enemyFactory, DeliveredParcelsCounter counter )
    {
        _enemyFactory = enemyFactory;
        _counter = counter;
    }

    public GameObject CreatePlayerAt(GameObject at, IInputService input)
    {
        PlayerGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
        PlayerGameObject.GetComponent<PlayerMove>().Init(input);
        PlayerGameObject.GetComponent<PlayerAttack>().Init(input);
        PlayerCreated?.Invoke();
        return PlayerGameObject;
    }

    public GameObject CreateHud()
    {
        var hud = InstantiateRegistered(AssetPath.HUDPath);
        hud.GetComponent<EnemiesCount>().SetEnemyFactory(_enemyFactory);
        hud.GetComponent<DeliveredParcelsPanel>().SetCounter(_counter);
        return hud;
    }

    public StartMenu CreateStartMenu()
    {
        var menu = _assets.Instantiate(AssetPath.StartMenuPath);
       
        return menu.GetComponent<StartMenu>();
    }

    public void CleanUp()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath, at);

        RegisterProgressWatchers(gameObject);
        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath);

        RegisterProgressWatchers(gameObject);
        return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            Register(progressReader);
    }

    public void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is ISavedProgress progressWriter)
            ProgressWriters.Add(progressWriter);

        ProgressReaders.Add(progressReader);
    }
}