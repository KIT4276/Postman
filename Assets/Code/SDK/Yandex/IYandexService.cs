using System;

public interface IYandexService
{
    event Action SdkReady;
    event Action GameApiPaused;
    event Action GameApiResumed;

    bool HasPlayer { get; }
    bool IsSdkReady { get; }

    void Init();
    void ReadyOnce();
    void TryShowInterstitial();
    void StartFallbackRewarded(Action onReward, Action<bool> onCompleted = null);
    void ShowRewarded(Action onReward, Action<bool> onCompleted = null);
}
