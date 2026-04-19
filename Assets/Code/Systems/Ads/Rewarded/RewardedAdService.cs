public class RewardedAdService : IRewardedAdService
{
    private readonly IYandexService _yandexService;
    private readonly Healing _healing;

    public event System.Action RewardGranted;
    public event System.Action<bool> RewardFlowCompleted;

    public bool IsBusy { get; private set; }

    public RewardedAdService(IYandexService yandexService, Healing healing)
    {
        _yandexService = yandexService;
        _healing = healing;
    }

    public bool TryShowHealReward()
    {
        if (IsBusy)
            return false;

        IsBusy = true;
        _yandexService.ShowRewarded(
            onReward: () =>
            {
                _healing.ToHeal();
                RewardGranted?.Invoke();
            },
            onCompleted: rewarded =>
            {
                IsBusy = false;
                RewardFlowCompleted?.Invoke(rewarded);
            });

        return true;
    }
}
