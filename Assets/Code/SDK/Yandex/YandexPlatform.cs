using System;
using UnityEngine;

public class YandexPlatform : MonoBehaviour
{
    private bool _initRequested;

    public bool IsSdkReady { get; private set; }
    public bool HasPlayer { get; private set; }

    public event Action SdkReady;
    public event Action<bool> PlayerReady;
    public event Action AdOpened;
    public event Action<bool> AdClosed;
    public event Action RvOpened;
    public event Action RvRewarded;
    public event Action RvClosed;
    public event Action RvFailed;
    public event Action<string> LanguageDetected;

    public void Init()
    {
        if (_initRequested)
        {
            if (IsSdkReady)
            {
                SdkReady?.Invoke();
                LanguageDetected?.Invoke(GetFallbackLanguage());
                PlayerReady?.Invoke(HasPlayer);
            }

            return;
        }

        _initRequested = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        YandexSDKBridge.Init(gameObject.name);
#else
        IsSdkReady = true;
        HasPlayer = true;
        SdkReady?.Invoke();
        LanguageDetected?.Invoke(GetFallbackLanguage());
        PlayerReady?.Invoke(HasPlayer);
#endif
    }

    public void Ready() => YandexSDKBridge.Ready();

    public void ShowInterstitial()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexSDKBridge.ShowInterstitial();
#else
        AdOpened?.Invoke();
        AdClosed?.Invoke(true);
#endif
    }

    public void ShowRewarded()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexSDKBridge.ShowRewarded();
#else
        RvOpened?.Invoke();
        RvRewarded?.Invoke();
        RvClosed?.Invoke();
#endif
    }

    public void OnLanguageDetected(string lang) => LanguageDetected?.Invoke(lang);

    public void OnYsdkInitOk(string _)
    {
        if (IsSdkReady)
            return;

        IsSdkReady = true;
        Debug.Log("[YandexPlatform] SDK ready");
        SdkReady?.Invoke();
    }

    public void OnYsdkInitError(string message)
    {
        IsSdkReady = false;
        Debug.LogWarning($"[YandexPlatform] SDK init failed: {message}");
    }

    public void OnPlayerReady(string hasPlayer)
    {
        HasPlayer = hasPlayer == "1";
        PlayerReady?.Invoke(HasPlayer);
    }

    public void OnAdOpen(string _) => AdOpened?.Invoke();
    public void OnAdClose(string wasShown) => AdClosed?.Invoke(wasShown == "1");

    public void OnAdError(string error)
    {
        Debug.LogWarning($"[YandexPlatform] Interstitial error: {error}");
        AdClosed?.Invoke(false);
    }

    public void OnRvOpen(string _) => RvOpened?.Invoke();
    public void OnRvReward(string _) => RvRewarded?.Invoke();
    public void OnRvClose(string _) => RvClosed?.Invoke();

    public void OnRvError(string error)
    {
        Debug.LogWarning($"[YandexPlatform] Rewarded error: {error}");
        RvFailed?.Invoke();
        RvClosed?.Invoke();
    }

    private static string GetFallbackLanguage()
    {
        return Application.systemLanguage == SystemLanguage.Russian ? "ru" : "en";
    }
}
