using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    [Serializable]
    public struct PurchaseConfig
    {
        /// <summary>
        /// ID of the product.
        /// </summary>
        [JsonProperty("productID")]
        public string ProductID;
        /// <summary>
        /// Optional payload assigned by game developer, which will be also attached in the signed purchase request.
        /// </summary>
        [JsonProperty("developerPayload", NullValueHandling = NullValueHandling.Ignore)]
        public string DeveloperPayload;
    }
}
