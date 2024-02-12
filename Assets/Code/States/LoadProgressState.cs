public class LoadProgressState : IState
{
    private const string Main = "Main";
    private readonly StateMachine _gameStateMachine;
    private readonly IPersistantProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;

    public LoadProgressState(StateMachine gameStateMachine, IPersistantProgressService progressService, ISaveLoadService saveLoadService)
    {
        _gameStateMachine = gameStateMachine;
        _progressService = progressService;
        _saveLoadService = saveLoadService;
    }

    public void Enter()
    {
        LoadProgressOrInitNew();

        _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {

    }

    private void LoadProgressOrInitNew() =>
        _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

    private PlayerProgress NewProgress()
    {
        var progress = new PlayerProgress(initialLevel: Main);

        progress.HeroState.MaxHP = 50;
        progress.HeroStats.Damage = 1f;
        progress.HeroStats.DamageRadius = 0.5f;

        progress.HeroState.ResetHP();

        return progress;
    }
}