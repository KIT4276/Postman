using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFactory : IService
{
    public event Action PlayerCreated;

    public GameObject PlayerGameObject { get; private set; }

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private readonly EnemyFactory _enemyFactory;
    private readonly DeliveredParcelsCounter _counter;
    private readonly Salary _salary;
    private readonly Healing _healing;
    private readonly IAssets _assets;
    private readonly PersistantStaticData _staticData;
    private readonly PersistantPlayerStaticData _playerStaticData;
    private readonly Experience _experience;
    private readonly IRewardedAdService _rewardedAdService;
    private readonly ILocalizationService _localizationService;

    public GameFactory(EnemyFactory enemyFactory, IAssets assets, DeliveredParcelsCounter counter,
        Salary salary, Healing healing, PersistantStaticData staticData, PersistantPlayerStaticData playerStaticData, 
        Experience experience, IRewardedAdService rewardedAdService, ILocalizationService localizationService)
    {
        _enemyFactory = enemyFactory;
        _assets = assets;
        _counter = counter;
        _salary = salary;
        _healing = healing;
        _staticData = staticData;
        _playerStaticData = playerStaticData;
        _experience = experience;
        _rewardedAdService = rewardedAdService;
        _localizationService = localizationService;
    }

    public GameObject CreatePlayerAt(GameObject at, IInputService input)
    {
        PlayerGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
        PlayerMove playerMove = PlayerGameObject.GetComponent<PlayerMove>();
        PlayerAttack playerAttack = PlayerGameObject.GetComponent<PlayerAttack>();
        PlayerHealing playerHealing = PlayerGameObject.GetComponent<PlayerHealing>();
        PlayerInfection playerInfection = PlayerGameObject.GetComponent<PlayerInfection>();

        playerMove.Init(input);
        playerAttack.Init(input);
        playerHealing.SetHealing(_healing);
        playerInfection.Init(_playerStaticData);
        PlayerCreated?.Invoke();
        return PlayerGameObject;
    }

    public GameObject CreateHud()
    {
        var hud = InstantiateRegistered(AssetPath.HUDPath);
        PlayerInfection playerInfection = PlayerGameObject.GetComponent<PlayerInfection>();
        PlayerHealth playerHealth = PlayerGameObject.GetComponent<PlayerHealth>();
        PlayerAttack playerAttack = PlayerGameObject.GetComponent<PlayerAttack>();
        EnemiesCount enemiesCount = hud.GetComponent<EnemiesCount>();
        MoneyPanel moneyPanel = hud.GetComponent<MoneyPanel>();
        DeliveredParcelsPanel deliveredParcelsPanel = hud.GetComponent<DeliveredParcelsPanel>();
        InfectionPanel infectionPanel = hud.GetComponent<InfectionPanel>();
        HealButton healButton = hud.GetComponent<HealButton>();
        ExperienceUI experienceUI = hud.GetComponent<ExperienceUI>();
        ActorUI actorUI = hud.GetComponent<ActorUI>();

        enemiesCount?.SetEnemyFactory(_enemyFactory);
        moneyPanel?.SetSalary(_salary);
        deliveredParcelsPanel?.SetCounter(_counter);
        infectionPanel?.SetInfection(playerInfection);
        Localize(hud);
        healButton?.Init(_salary, _healing, _staticData, _rewardedAdService, _localizationService);
        experienceUI?.Init(_experience);
        actorUI?.Construct(playerHealth);
        InitAttackButton(hud, playerAttack);
        Register(_experience);       
        return hud;
    }

    public StartMenu CreateStartMenu()
    {
        StartMenu startMenu = _assets.Instantiate(AssetPath.StartMenuPath).GetComponent<StartMenu>();
        Localize(startMenu.gameObject);
        return startMenu;
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

    private void Localize(GameObject gameObject)
    {
        LocalizedTextGroup localizedTextGroup = gameObject.GetComponent<LocalizedTextGroup>();

        if (localizedTextGroup == null)
            localizedTextGroup = gameObject.AddComponent<LocalizedTextGroup>();

        localizedTextGroup.Init(_localizationService);
    }

    private void InitAttackButton(GameObject hud, PlayerAttack playerAttack)
    {
        Button attackButton = FindChildComponent<Button>(hud, "AttackButton");

        if (attackButton == null || playerAttack == null)
            return;

        attackButton.onClick.RemoveListener(playerAttack.Attack);
        attackButton.onClick.AddListener(playerAttack.Attack);
    }

    private T FindChildComponent<T>(GameObject root, string childName) where T : Component
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            if (child.name == childName && child.TryGetComponent(out T component))
                return component;

        return null;
    }
}
