﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFactory : IService
{
    public event Action PlayerCreated;

    public GameObject PlayerGameObject { get; private set; }

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private readonly EnemyFactory _enemyFactory;
    private readonly DeliveredParcelsCounter _counter;
    private readonly Salary _salary;
    private readonly IAssets _assets;

    public GameFactory(EnemyFactory enemyFactory, IAssets assets, DeliveredParcelsCounter counter,
        Salary salary)
    {
        _enemyFactory = enemyFactory;
        _assets = assets;
        _counter = counter;
        _salary = salary;
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
        hud.GetComponent<MoneyPanel>().SetSalary(_salary);
        hud.GetComponent<DeliveredParcelsPanel>().SetCounter(_counter);
        hud.GetComponent<InfectionPanel>().SetInfection(PlayerGameObject.GetComponent<PlayerInfection>());
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