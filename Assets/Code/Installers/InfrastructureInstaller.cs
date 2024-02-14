using System;
using UnityEngine;
using Zenject;

public class InfrastructureInstaller : MonoInstaller, ICoroutineRunner
{
    [SerializeField]
    private GameObject _curtainPrefab;
    [SerializeField]
    private GameObject _enterPointPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _aidPrefab;
    [SerializeField]
    private GameObject _entenemiesParentPrefab;
    [SerializeField]
    private GameObject _aidParentPrefab;
    [SerializeField]
    private int _EnemyesPoolCount = 20;

    private const string Curtain = "Curtain";
    private const string Infrastructure = "Infrastructure";
    private const string EnterPoint = "EnterPoint";
    private const string Entenemies = "Entenemies";
    private const string Gameplay = "Gameplay";
    private const string AIDs = "AIDs";

    public override void InstallBindings()
    {
        InstallInputService();
        InstallEnemiesParent();
        InstallAIDParent();

        this.gameObject.SetActive(true);
        Container.BindInterfacesAndSelfTo<ICoroutineRunner>().FromInstance(this).AsSingle();

       InstallSceneLoader();

        BindFactories();
        BindServices();

        BindEnterPoint();
        
    }

   

    private void InstallAIDParent()
    {
        Container.BindInterfacesAndSelfTo<AIDParent>().FromComponentInNewPrefab(_aidParentPrefab).
            WithGameObjectName(AIDs).UnderTransformGroup(Gameplay).AsSingle().NonLazy();
    }

    private void InstallEnemiesParent()
    {
        Container.BindInterfacesAndSelfTo<EntenemiesParent>().FromComponentInNewPrefab(_entenemiesParentPrefab).
            WithGameObjectName(Entenemies).UnderTransformGroup(Gameplay).AsSingle().NonLazy();
    }

    private void BindEnterPoint()
    {
        Container.BindInterfacesAndSelfTo<EnterPoint>().FromComponentInNewPrefab(_enterPointPrefab).
            WithGameObjectName(EnterPoint).UnderTransformGroup(Infrastructure).AsSingle().NonLazy();
    }



    private void InstallInputService()
    {
        IInputService input = DefineInputService();
        Container.BindInterfacesAndSelfTo<IInputService>().FromInstance(input).AsSingle().NonLazy();
    }

    private void InstallSceneLoader() =>
        Container.Bind<SceneLoader>().FromNew().AsSingle().WithArguments(this).NonLazy();

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<LoadingCurtain>().FromComponentInNewPrefab(_curtainPrefab).
            WithGameObjectName(Curtain).UnderTransformGroup(Infrastructure).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AssetsProvider>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PersistantProgressService>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SaveLoadService>().FromNew().AsSingle().NonLazy();
    }

    private void BindFactories()
    {
        Container.Bind<EnemyFactory>().AsSingle();
        Container.BindMemoryPool<Enemy, Enemy.Pool>().WithInitialSize(_EnemyesPoolCount).FromComponentInNewPrefab(_enemyPrefab);

        Container.Bind<AIDFactory>().AsSingle();
        Container.BindMemoryPool<AIDTrigger, AIDTrigger.Pool>().FromComponentInNewPrefab(_aidPrefab);

        Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameFactory>().AsSingle().NonLazy();
    }

    private static IInputService DefineInputService()
    {
        if (Application.isEditor)
            return new StandaloneInputService();
        else
            return new MobileInputService();
    }
}
