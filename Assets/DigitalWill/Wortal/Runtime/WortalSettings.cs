using UnityEngine;

namespace DigitalWill.H5Portal
{
    /// <summary>
    /// Settings for the wortal.
    /// </summary>
    public class WortalSettings : ScriptableObject
    {
        [Header("Link Configuration")]
        [SerializeField] private string _linkInterstitialId;
        [SerializeField] private string _linkRewardedId;

        public string LinkInterstitialId => _linkInterstitialId;
        public string LinkRewardedId => _linkRewardedId;
    }
}
