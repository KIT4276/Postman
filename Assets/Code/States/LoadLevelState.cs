using UnityEngine;

public class LoadLevelState : IPayloadedState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private const string EnemySpawnerTag = "EnemySpawner";
    
    private const string AIDSpawnerTAg = "AIDSpawner";
    private const string AddressTag = "Address";
    private readonly StateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly GameFactory _gameFactory;
    private readonly IInputService _input;
    private IPersistantProgressService _progressService;
    private GameObject _playerObj;
    private Post _addresses;
    private ParcelGenerator _parcelGenerator;
    private readonly DeliveredParcelsCounter _counter;
    private readonly MaintenanceEnemyesCount _maintenanceEC;
    private readonly MaintenanceAIDCount _maintenanceAC;
    private readonly Salary _salary;

    public LoadLevelState(StateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain,
        GameFactory gameFactory, IInputService input, IPersistantProgressService progressService, Post addresses, 
        ParcelGenerator parcelGenerator, DeliveredParcelsCounter counter, MaintenanceEnemyesCount maintenanceEC,
        MaintenanceAIDCount maintenanceAC, Salary salary)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _input = input;
        _progressService = progressService;
        _addresses = addresses;
        _parcelGenerator = parcelGenerator;
        _counter = counter;
        _maintenanceEC = maintenanceEC;
        _maintenanceAC = maintenanceAC;
        _salary = salary;
    }

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _gameFactory.CleanUp();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
        _curtain.Hide();

    private void OnLoaded()
    {
        InitGameWorld();
        InformProgressReaders();

        _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld()
    {
        _playerObj = InitPlayer();
        InitHud(_playerObj);

        InitSpawners();
        _maintenanceEC.SetSpawners();
        _maintenanceAC.SetSpawners();
        InitAddressTriggers();

        CameraFollow(_playerObj);
    }

    private void InitAddressTriggers()
    {
        if (GameObject.FindGameObjectsWithTag(AddressTag) != null)
        {
            foreach (var address in GameObject.FindGameObjectsWithTag(AddressTag))
            {
                _addresses.SetAddress(address.GetComponent<AddressTrigger>());
            }
            _gameFactory.Register(_counter);
            _gameFactory.Register(_salary);
            _parcelGenerator.StartGenerate();
        }
    }

    private void InitSpawners()
    {
        if (GameObject.FindGameObjectsWithTag(EnemySpawnerTag) != null)
            Init(EnemySpawnerTag);

        if (GameObject.FindGameObjectsWithTag(AIDSpawnerTAg) != null)
            Init(AIDSpawnerTAg);
    }

    private void Init(string tag)
    {
        foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(tag))
        {
            var spawner = spawnerObject.GetComponent<Spawner>();
            _gameFactory.Register(spawner);
        }
    }

    private GameObject InitPlayer()
    {
        return _gameFactory.CreatePlayerAt(GameObject.FindWithTag(InitialPointTag), _input);
    }

    private void InitHud(GameObject player)
    {
        GameObject hud = _gameFactory.CreateHud();
        hud.GetComponentInChildren<ActorUI>().Construct(player.GetComponent<PlayerHealth>());
    }

    private void CameraFollow(GameObject player) =>
        Camera.main.GetComponent<CameraFollow>().Follow(player.transform);
}
