using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField]
    private PersistantStaticData _persistantStaticData;

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
    }

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