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
    private readonly IInputService _inputService;

    private bool _isInitialized;
    private bool _readyRequested;
    private bool _readySent;
    private bool _sdkReadyRaised;
    private bool _rewardedInProgress;
    private bool _gameApiPauseRequested;
    private bool _adPauseRequested;
    private bool _platformPauseApplied;
    private bool _audioListenerPausedBeforeGameApiPause;
    private bool _inputEnabledBeforeGameApiPause;
    private float _timeScaleBeforeGameApiPause;
    private float _nextInterstitialTime;

    public event Action SdkReady;
    public event Action GameApiPaused;
    public event Action GameApiResumed;

    public bool HasPlayer => _platform != null && _platform.HasPlayer;
    public bool IsSdkReady => _platform != null && _platform.IsSdkReady;

    public YandexService(YandexPlatform platform, ILocalizationService localizationService, IInputService inputService)
    {
        _platform = platform;
        _localizationService = localizationService;
        _inputService = inputService;
    }

    public void Initialize()
    {
        if (_platform == null)
            return;

        _platform.SdkReady += OnPlatformSdkReady;
        _platform.LanguageDetected += OnLanguageDetected;
        _platform.GameApiPaused += OnGameApiPaused;
        _platform.GameApiResumed += OnGameApiResumed;
        _platform.AdOpened += OnAdOpened;
        _platform.AdClosed += OnAdClosed;
        _platform.RvOpened += OnRewardedOpened;
        _platform.RvClosed += OnRewardedClosed;

        if (_platform.IsSdkReady)
            OnPlatformSdkReady();
    }

    public void Dispose()
    {
        if (_platform == null)
            return;

        _platform.SdkReady -= OnPlatformSdkReady;
        _platform.LanguageDetected -= OnLanguageDetected;
        _platform.GameApiPaused -= OnGameApiPaused;
        _platform.GameApiResumed -= OnGameApiResumed;
        _platform.AdOpened -= OnAdOpened;
        _platform.AdClosed -= OnAdClosed;
        _platform.RvOpened -= OnRewardedOpened;
        _platform.RvClosed -= OnRewardedClosed;
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
        _readyRequested = true;

        if (_readySent)
            return;

        if (!IsSdkReady)
            return;

        SendReady();
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
        if (_readyRequested)
            SendReady();

        if (_sdkReadyRaised)
            return;

        _sdkReadyRaised = true;
        SdkReady?.Invoke();
    }

    private void SendReady()
    {
        if (_readySent)
            return;

        _readySent = true;
        _platform?.Ready();
    }

    private void OnLanguageDetected(string languageCode)
    {
        _localizationService.ApplySdkLanguage(languageCode);
    }

    private void OnGameApiPaused()
    {
        if (_gameApiPauseRequested)
            return;

        _gameApiPauseRequested = true;
        ApplyPlatformPause();
        GameApiPaused?.Invoke();
    }

    private void OnGameApiResumed()
    {
        if (!_gameApiPauseRequested)
            return;

        _gameApiPauseRequested = false;
        TryReleasePlatformPause();
        GameApiResumed?.Invoke();
    }

    private void OnAdOpened() => RequestAdPause();

    private void OnAdClosed(bool _) => ReleaseAdPause();

    private void OnRewardedOpened() => RequestAdPause();

    private void OnRewardedClosed() => ReleaseAdPause();

    private void RequestAdPause()
    {
        if (_adPauseRequested)
            return;

        _adPauseRequested = true;
        ApplyPlatformPause();
    }

    private void ReleaseAdPause()
    {
        if (!_adPauseRequested)
            return;

        _adPauseRequested = false;
        TryReleasePlatformPause();
    }

    private void ApplyPlatformPause()
    {
        if (_platformPauseApplied)
            return;

        _platformPauseApplied = true;
        _timeScaleBeforeGameApiPause = Time.timeScale;
        _audioListenerPausedBeforeGameApiPause = AudioListener.pause;
        _inputEnabledBeforeGameApiPause = _inputService == null || _inputService.IsEnabled;

        Time.timeScale = 0f;
        AudioListener.pause = true;
        if (_inputService != null)
            _inputService.IsEnabled = false;
    }

    private void TryReleasePlatformPause()
    {
        if (!_platformPauseApplied || _gameApiPauseRequested || _adPauseRequested)
            return;

        Time.timeScale = _timeScaleBeforeGameApiPause;
        AudioListener.pause = _audioListenerPausedBeforeGameApiPause;
        if (_inputService != null)
            _inputService.IsEnabled = _inputEnabledBeforeGameApiPause;

        _platformPauseApplied = false;
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
