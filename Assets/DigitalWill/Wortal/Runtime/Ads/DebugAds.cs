namespace DigitalWill
{
    /// <summary>
    /// Debug mode implementation of <see cref="IAdProvider"/>. Does not return any ads, only calls stubs and fires
    /// events.
    /// </summary>
    public class DebugAds : IAdProvider
    {
        public void ShowInterstitialAd(Placement type, string name)
        {
            Wortal.CallBeforeAd();
            Wortal.CallAfterAd();
        }

        /// <summary>
        /// Debug rewarded ad - allows for testing both AdViewed and AdDismissed params.
        /// </summary>
        /// <param name="name">"true" to trigger the AdViewed event or "false" to trigger the AdDismissed event.</param>
        public void ShowRewardedAd(string name)
        {
            Wortal.CallBeforeAd();

            switch (name)
            {
                case "true":
                    Wortal.CallAdViewed();
                    break;
                case "false":
                    Wortal.CallAdDismissed();
                    break;
                default:
                    Wortal.CallAdViewed();
                    break;
            }

            Wortal.CallAfterAd();
        }
    }
}
