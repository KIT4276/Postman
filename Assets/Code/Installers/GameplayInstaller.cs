using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PersistantStaticData _persistantStaticData;

    [SerializeField] private float _targetXP = 100;
    [SerializeField] private int _xpIncreaseStep = 10;
    [SerializeField] private int _xpForDeliver = 20;
    [SerializeField] private int _xpForKilling = 10;

    public override void InstallBindings()
    {
        InstallScriptableObjects();

        InstallPost();

        InstallParcelGenerator();
        InstallDeliveredParcelsCounter();
        InstallSalary();

        InstallMaintenanceEnemyesCount();
        InstallMaintenanceAIDCount();

        InstallHealing();

        BindExperience();
    }


    private void BindExperience() =>
        Container.BindInterfacesAndSelfTo<Experience>().FromNew().AsSingle().
        WithArguments(_targetXP, _xpIncreaseStep, _xpForDeliver, _xpForKilling).NonLazy();

    private void InstallScriptableObjects() =>
        Container.Bind<PersistantStaticData>().FromInstance(_persistantStaticData).AsSingle().NonLazy();

    private void InstallHealing() =>
        Container.BindInterfacesAndSelfTo<Healing>().FromNew().AsSingle().NonLazy();

    private void InstallSalary() =>
        Container.BindInterfacesAndSelfTo<Salary>().FromNew().AsSingle().NonLazy();

    private void InstallMaintenanceAIDCount() =>
       Container.BindInterfacesAndSelfTo<MaintenanceAIDCount>().FromNew().AsSingle().NonLazy();

    private void InstallMaintenanceEnemyesCount() =>
        Container.BindInterfacesAndSelfTo<MaintenanceEnemyesCount>().FromNew().AsSingle().NonLazy();

    private void InstallDeliveredParcelsCounter() =>
        Container.BindInterfacesAndSelfTo<DeliveredParcelsCounter>().FromNew().AsSingle().NonLazy();

    private void InstallParcelGenerator() =>
        Container.BindInterfacesAndSelfTo<ParcelGenerator>().FromNew().AsSingle().NonLazy();

    private void InstallPost() =>
        Container.BindInterfacesAndSelfTo<Post>().FromNew().AsSingle().NonLazy();
}