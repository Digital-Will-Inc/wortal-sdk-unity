using System;
using AOT;
using System.Runtime.InteropServices;

namespace AFG.Api {
  public static class InterstitialAdType {
    public const string Start = "start";
    public const string Pause = "pause";
    public const string Next = "next";
    public const string Browse = "browse";
  }

  public class AdBreak {
    public delegate void BeforeAdDelegate();
    public delegate void AfterAdDelegate();
    public delegate void AdBreakDoneDelegate();
    public delegate void BeforeRewardDelegate();
    public delegate void AdDismissedDelegate();
    public delegate void AdViewedDelegate();
    [DllImport("__Internal")]
    private static extern void TriggerBeforeAd();
    [DllImport("__Internal")]
    private static extern void TriggerAfterAd();
    [DllImport("__Internal")]
    private static extern void TriggerAdBreakDone();
    [DllImport("__Internal")]
    private static extern void TriggerBeforeReward();
    [DllImport("__Internal")]
    private static extern void TriggerAdDismissed();
    [DllImport("__Internal")]
    private static extern void TriggerAdViewed();
    [DllImport("__Internal")]
    private static extern void ShowRewardedAdViaJS();
    [DllImport("__Internal")]
    private static extern void ShowInterstitialAdViaJS(String type, String name,
                                                       BeforeAdDelegate beforeAdCallback,
                                                       AfterAdDelegate afterAdCallback,
                                                       AdBreakDoneDelegate adBreakDoneDelegate);
    [DllImport("__Internal")]
    private static extern void RequestRewardedAdViaJS(String name, BeforeAdDelegate beforeAdCallback,
                                                   AfterAdDelegate afterAdCallback,
                                                   AdBreakDoneDelegate adBreakDoneDelegate,
                                                   BeforeRewardDelegate beforeRewardDelegate,
                                                   AdDismissedDelegate adDismissedDelegate,
                                                   AdViewedDelegate adViewedDelegate);

    public static void ShowInterstitialAd(String type, String name,
                                          BeforeAdDelegate beforeAdCallback,
                                          AfterAdDelegate afterAdCallback,
                                          AdBreakDoneDelegate adBreakDoneDelegate) {
      ShowInterstitialAdViaJS(type, name, beforeAdCallback, afterAdCallback, adBreakDoneDelegate);
    }

    public static void RequestRewardedAd(String name, BeforeAdDelegate beforeAdCallback,
                                      AfterAdDelegate afterAdCallback,
                                      AdBreakDoneDelegate adBreakDoneDelegate,
                                      BeforeRewardDelegate beforeRewardDelegate,
                                      AdDismissedDelegate adDismissedDelegate,
                                      AdViewedDelegate adViewedDelegate) {
      RequestRewardedAdViaJS(name, beforeAdCallback, afterAdCallback, adBreakDoneDelegate,
                          beforeRewardDelegate, adDismissedDelegate, adViewedDelegate);
    }
    public static void ShowRewardedAd() {
      ShowRewardedAdViaJS();
    }
  }
}
