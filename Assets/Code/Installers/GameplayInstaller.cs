using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallPost();
        InstallParcelGenerator();
        InstallDeliveredParcelsCounter();

        InstallMaintenanceEnemyesCount();
        InstallMaintenanceAIDCount();
    }

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