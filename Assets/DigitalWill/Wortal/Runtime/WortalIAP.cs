using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// In-App Purchasing API
    /// </summary>
    public class WortalIAP
    {
        private static Action<Product[]> _getCatalogCallback;
        private static Action<Purchase[]> _getPurchasesCallback;
        private static Action<Purchase> _makePurchaseCallback;
        private static Action _consumePurchaseCallback;

#region Public API

        /// <summary>
        /// Checks whether IAP is enabled in this session.
        /// </summary>
        /// <returns>True if IAP is available to the user. False if IAP is not supported on the current platform,
        /// the player's device, or the IAP service failed to load properly.</returns>
        public bool IsEnabled()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IAPIsEnabledJS();
#else
            Debug.Log("[Wortal] Mock IAP.IsEnabled()");
            return true;
#endif
        }

        /// <summary>
        /// Gets the catalog of available products the player can purchase.
        /// </summary>
        /// <param name="callback">Callback with array of products representing the IAP catalog. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.IAP.GetCatalog(
        ///     catalog => Debug.Log(catalog[0].Title),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>PAYMENTS_NOT_INITIALIZED</li>
        /// <li>NETWORK_FAILURE</li>
        /// </ul></throws>
        public void GetCatalog(Action<Product[]> callback, Action<WortalError> errorCallback)
        {
            _getCatalogCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPGetCatalogJS(IAPGetCatalogCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock IAP.GetCatalog()");
            var product = new Product
            {
                Title = "MyProduct",
                ProductID = "mock.product.id",
                Description = "A really cool product",
                ImageURI = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                Price = "10",
                PriceCurrencyCode = "USD",
            };
            Product[] products = { product };
            IAPGetCatalogCallback(JsonConvert.SerializeObject(products));
#endif
        }

        /// <summary>
        /// Gets the purchases the player has made that have not yet been consumed. Purchase signature should be
        /// validated on the game developer's server or transaction database before provisioning the purchase to the player.
        /// </summary>
        /// <param name="callback">Callback with array of purchases the player owns. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.IAP.GetPurchases(
        ///     purchases => Debug.Log(purchases[0].PurchaseToken), // Use this token to consume purchase
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>PAYMENTS_NOT_INITIALIZED</li>
        /// <li>NETWORK_FAILURE</li>
        /// </ul></throws>
        public void GetPurchases(Action<Purchase[]> callback, Action<WortalError> errorCallback)
        {
            _getPurchasesCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPGetPurchasesJS(IAPGetPurchasesCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock IAP.GetPurchases()");
            var purchase = new Purchase
            {
                DeveloperPayload = "MyPayload",
                PaymentID = "XYZ123",
                ProductID = "mock.product.id",
                PurchaseTime = "1672098823",
                PurchaseToken = "abcd-1234-xyz",
                SignedRequest = "aBcDeF12g",
            };
            Purchase[] purchases = { purchase };
            IAPGetPurchasesCallback(JsonConvert.SerializeObject(purchases));
#endif
        }

        /// <summary>
        /// Attempts to make a purchase of the given product. Will launch the native IAP screen and return the result.
        /// </summary>
        /// <param name="purchase">Object defining the product ID and purchase information.</param>
        /// <param name="callback">Callback with info about the purchase, if successful. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.IAP.MakePurchase(new WortalIAP.PurchaseConfig
        ///     {
        ///         ProductID = "id.code.for.product",
        ///     },
        ///     purchase => Debug.Log(purchase.PurchaseToken), // Use this token to consume purchase
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>PAYMENTS_NOT_INITIALIZED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>USER_INPUT</li>
        /// </ul></throws>
        public void MakePurchase(PurchaseConfig purchase, Action<Purchase> callback, Action<WortalError> errorCallback)
        {
            _makePurchaseCallback = callback;
            Wortal.WortalError = errorCallback;
            string purchaseObj = JsonConvert.SerializeObject(purchase);
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPMakePurchaseJS(purchaseObj, IAPMakePurchaseCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock IAP.MakePurchase({purchase})");
            var purchaseReceipt = new Purchase
            {
                DeveloperPayload = "MyPayload",
                PaymentID = "XYZ123",
                ProductID = "mock.product.id",
                PurchaseTime = "1672098823",
                PurchaseToken = "abcd-1234-xyz",
                SignedRequest = "aBcDeF12g",
            };
            IAPMakePurchaseCallback(JsonConvert.SerializeObject(purchaseReceipt));
#endif
        }

        /// <summary>
        /// Consumes the given purchase. This will remove the purchase from the player's available purchases inventory and
        /// reset its availability in the catalog.
        /// </summary>
        /// <param name="token">String representing the PurchaseToken of the item to consume.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.IAP.ConsumePurchase("PurchaseToken",
        ///     () => DoSomethingWithConsumedPurchase(),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>PAYMENTS_NOT_INITIALIZED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// </ul></throws>
        public void ConsumePurchase(string token, Action callback, Action<WortalError> errorCallback)
        {
            _consumePurchaseCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPConsumePurchaseJS(token, IAPConsumePurchaseCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock IAP.ConsumePurchase({token})");
            IAPConsumePurchaseCallback();
#endif
        }

#endregion Public API
#region JSlib Interface

        [DllImport("__Internal")]
        private static extern bool IAPIsEnabledJS();

        [DllImport("__Internal")]
        private static extern void IAPGetCatalogJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void IAPGetPurchasesJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void IAPMakePurchaseJS(string purchase, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void IAPConsumePurchaseJS(string token, Action callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void IAPGetCatalogCallback(string catalog)
        {
            Product[] catalogObj = JsonConvert.DeserializeObject<Product[]>(catalog);
            _getCatalogCallback?.Invoke(catalogObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void IAPGetPurchasesCallback(string purchases)
        {
            Purchase[] purchasesObj = JsonConvert.DeserializeObject<Purchase[]>(purchases);
            _getPurchasesCallback?.Invoke(purchasesObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void IAPMakePurchaseCallback(string purchase)
        {
            Purchase purchaseObj = JsonConvert.DeserializeObject<Purchase>(purchase);
            _makePurchaseCallback?.Invoke(purchaseObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void IAPConsumePurchaseCallback()
        {
            _consumePurchaseCallback?.Invoke();
        }

#endregion JSlib Interface
#region Types

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

        [Serializable]
        public struct Purchase
        {
            /// <summary>
            /// Optional payload assigned by game developer, which will be also attached in the signed purchase request.
            /// </summary>
            [JsonProperty("developerPayload", NullValueHandling = NullValueHandling.Ignore)]
            public string DeveloperPayload;
            /// <summary>
            /// ID of the payment (e.g. Google Play Order).
            /// </summary>
            [JsonProperty("paymentID")]
            public string PaymentID;
            /// <summary>
            /// ID of the product.
            /// </summary>
            [JsonProperty("productID")]
            public string ProductID;
            /// <summary>
            /// Timestamp of the payment.
            /// </summary>
            [JsonProperty("purchaseTime")]
            public string PurchaseTime;
            /// <summary>
            /// Token for purchase consumption.
            /// </summary>
            [JsonProperty("purchaseToken")]
            public string PurchaseToken;
            /// <summary>
            /// A signature that can be verified on game's backend server.
            /// Server side validation can be done by following these steps:
            ///
            /// 1. Split the signature into two parts delimited by the `.` character.
            /// 2. Decode the first part with base64url encoding, which should be a hash.
            /// 3. Decode the second part with base64url encoding, which should be a string representation of an JSON object.
            /// 4. Hash the second part string using HMAC SHA-256 and the app secret, check if it is identical to the hash from step 2.
            /// 5. Optionally, developer can also validate the timestamp to see if the request is made recently.
            /// </summary>
            [JsonProperty("signedRequest")]
            public string SignedRequest;
        }

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

#endregion Types
    }
}
