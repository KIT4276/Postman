using System.Runtime.InteropServices;

public static class YandexSDKBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void YG_Init(string goName);
    [DllImport("__Internal")] private static extern void YG_Ready();
    [DllImport("__Internal")] private static extern void YG_ShowFullscreenAdv();
    [DllImport("__Internal")] private static extern void YG_ShowRewardedVideo();

    public static void Init(string goName) => YG_Init(goName);
    public static void Ready() => YG_Ready();
    public static void ShowInterstitial() => YG_ShowFullscreenAdv();
    public static void ShowRewarded() => YG_ShowRewardedVideo();
#else
    public static void Init(string goName) { }
    public static void Ready() { }
    public static void ShowInterstitial() { }
    public static void ShowRewarded() { }
#endif
}
