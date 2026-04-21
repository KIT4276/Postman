public class BootstrapState : IState
{
    private const string Initial = "Initial";

    private readonly StateMachine _stateMachine;
    private readonly IYandexService _yandexService;
    private readonly ILocalizationService _localizationService;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly GameFactory _gameFactory;

    public BootstrapState(StateMachine stateMachine, IYandexService yandexService,
        ILocalizationService localizationService, SceneLoader sceneLoader, LoadingCurtain curtain,
        GameFactory gameFactory)
    {
        _stateMachine = stateMachine;
        _yandexService = yandexService;
        _localizationService = localizationService;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
    }

    public void Enter()
    {
        _localizationService.InitializeFromSystemLanguage();
        _yandexService.Init();
        _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    public void Exit() { }

    private void EnterLoadLevel()
    {
        InstallStartMenu();
        _curtain.Hide();
        _yandexService.ReadyOnce();
    }

    private void InstallStartMenu() =>
        _gameFactory.CreateStartMenu().OnStarted += ContinueLoad;

    private void ContinueLoad()
    {
        _curtain.Show();
        _stateMachine.Enter<LoadProgressState>();
    }
}
