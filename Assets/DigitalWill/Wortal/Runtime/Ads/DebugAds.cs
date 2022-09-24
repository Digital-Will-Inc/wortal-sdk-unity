namespace DigitalWill
{
    /// <summary>
    /// Debug mode implementation of <see cref="IAdProvider"/>. Does not return any ads, only calls stubs and fires
    /// events.
    /// </summary>
    public class DebugAds : IAdProvider
    {
        public void ShowInterstitialAd(Placement type, string description)
        {
            Wortal.InvokeBeforeAd();
            Wortal.InvokeAfterAd();
        }

        /// <summary>
        /// Debug rewarded ad - allows for testing both AdViewed and AdDismissed params.
        /// </summary>
        /// <param name="description">"true" to trigger the AdViewed event or "false" to trigger the AdDismissed event.</param>
        public void ShowRewardedAd(string description)
        {
            Wortal.InvokeBeforeAd();

            switch (description)
            {
                case "true":
                    Wortal.InvokeRewardPlayer();
                    break;
                case "false":
                    Wortal.InvokeRewardSkipped();
                    break;
                default:
                    Wortal.InvokeRewardPlayer();
                    break;
            }

            Wortal.InvokeAfterAd();
        }
    }
}
