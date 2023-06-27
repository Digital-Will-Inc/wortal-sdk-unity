using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    [Serializable]
    public struct Product
    {
        /// <summary>
        /// Title of the product.
        /// </summary>
        [JsonProperty("title")]
        public string Title;
        /// <summary>
        /// ID of the product.
        /// </summary>
        [JsonProperty("productID")]
        public string ProductID;
        /// <summary>
        /// Text description of the product.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description;
        /// <summary>
        /// A URL to the product's image.
        /// </summary>
        [JsonProperty("imageURI", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageURI;
        /// <summary>
        /// A localized string representing the product's price in the local currency, e.g. "$1".
        /// </summary>
        [JsonProperty("price")]
        public string Price;
        /// <summary>
        /// A string representing which currency is the price calculated in, following [ISO 4217](https://en.wikipedia.org/wiki/ISO_4217).
        /// </summary>
        [JsonProperty("priceCurrencyCode")]
        public string PriceCurrencyCode;
    }
}
