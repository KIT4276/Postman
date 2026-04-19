using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class YandexService : IYandexService, IInitializable, IDisposable
{
    private const float InterstitialCooldownSeconds = 90f;
    private const float RewardedFallbackDelaySeconds = 3f;

    private readonly YandexPlatform _platform;
    private readonly ILocalizationService _localizationService;

    private bool _isInitialized;
    private bool _readySent;
    private bool _sdkReadyRaised;
    private bool _rewardedInProgress;
    private float _nextInterstitialTime;

    public event Action SdkReady;

    public bool HasPlayer => _platform != null && _platform.HasPlayer;
    public bool IsSdkReady => _platform != null && _platform.IsSdkReady;

    public YandexService(YandexPlatform platform, ILocalizationService localizationService)
    {
        _platform = platform;
        _localizationService = localizationService;
    }

    public void Initialize()
    {
        if (_platform == null)
            return;

        _platform.SdkReady += OnPlatformSdkReady;
        _platform.LanguageDetected += OnLanguageDetected;

        if (_platform.IsSdkReady)
            OnPlatformSdkReady();
    }

    public void Dispose()
    {
        if (_platform == null)
            return;

        _platform.SdkReady -= OnPlatformSdkReady;
        _platform.LanguageDetected -= OnLanguageDetected;
    }

    public void Init()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;
        _platform?.Init();
    }

    public void ReadyOnce()
    {
        if (_readySent)
            return;

        _readySent = true;
        _platform?.Ready();
    }

    public void TryShowInterstitial()
    {
        if (!IsSdkReady)
            return;

        if (Time.unscaledTime < _nextInterstitialTime)
            return;

        _nextInterstitialTime = Time.unscaledTime + InterstitialCooldownSeconds;
        _platform.ShowInterstitial();
    }

    public void ShowRewarded(Action onReward, Action<bool> onCompleted = null)
    {
        if (_rewardedInProgress)
        {
            onCompleted?.Invoke(false);
            return;
        }

#if UNITY_EDITOR
        StartFallbackRewarded(onReward, onCompleted);
        return;
#endif
        if (_platform == null || !_platform.IsSdkReady)
        {
            StartFallbackRewarded(onReward, onCompleted);
            return;
        }

        _rewardedInProgress = true;
        bool rewardGranted = false;
        bool rewardFailed = false;

        void OnRewardGranted()
        {
            rewardGranted = true;
        }

        void OnRewardFailed()
        {
            rewardFailed = true;
        }

        void OnRewardClosed()
        {
            _platform.RvRewarded -= OnRewardGranted;
            _platform.RvFailed -= OnRewardFailed;
            _platform.RvClosed -= OnRewardClosed;
            _rewardedInProgress = false;

            if (rewardGranted)
            {
                CompleteRewarded(onReward, onCompleted, true);
                return;
            }

            if (rewardFailed)
            {
                StartFallbackRewarded(onReward, onCompleted);
                return;
            }

            onCompleted?.Invoke(false);
        }

        _platform.RvRewarded += OnRewardGranted;
        _platform.RvFailed += OnRewardFailed;
        _platform.RvClosed += OnRewardClosed;
        _platform.ShowRewarded();
    }

    public void StartFallbackRewarded(Action onReward, Action<bool> onCompleted = null)
    {
        if (_rewardedInProgress)
        {
            onCompleted?.Invoke(false);
            return;
        }

        _platform.StartCoroutine(SimulateFallbackRewarded(onReward, onCompleted));
    }

    private void OnPlatformSdkReady()
    {
        ReadyOnce();

        if (_sdkReadyRaised)
            return;

        _sdkReadyRaised = true;
        SdkReady?.Invoke();
    }

    private void OnLanguageDetected(string languageCode)
    {
        _localizationService.ApplySdkLanguage(languageCode);
    }

    private IEnumerator SimulateFallbackRewarded(Action onReward, Action<bool> onCompleted)
    {
        _rewardedInProgress = true;
        yield return new WaitForSecondsRealtime(RewardedFallbackDelaySeconds);
        _rewardedInProgress = false;
        CompleteRewarded(onReward, onCompleted, true);
    }

    private static void CompleteRewarded(Action onReward, Action<bool> onCompleted, bool rewarded)
    {
        if (rewarded)
            onReward?.Invoke();

        onCompleted?.Invoke(rewarded);
    }
}
