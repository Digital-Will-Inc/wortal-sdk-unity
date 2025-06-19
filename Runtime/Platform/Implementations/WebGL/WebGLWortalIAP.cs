using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalIAP : IWortalIAP
    {
        private static Action<Product[]> _getCatalogCallback;
        private static Action<Purchase[]> _getPurchasesCallback;
        private static Action<Purchase> _makePurchaseCallback;
        private static Action _consumePurchaseCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public void GetCatalogAsync(Action<Product[]> onSuccess, Action<WortalError> onError)
        {
            _getCatalogCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPGetCatalogJS(IAPGetCatalogCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock IAP.GetCatalogAsync()");
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

        public void GetPurchasesAsync(Action<Purchase[]> onSuccess, Action<WortalError> onError)
        {
            _getPurchasesCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPGetPurchasesJS(IAPGetPurchasesCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock IAP.GetPurchasesAsync()");
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

        public void MakePurchaseAsync(PurchaseConfig purchaseConfig, Action<Purchase> onSuccess, Action<WortalError> onError)
        {
            _makePurchaseCallback = onSuccess;
            _errorCallback = onError;
            string purchaseObj = JsonConvert.SerializeObject(purchaseConfig);
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPMakePurchaseJS(purchaseObj, IAPMakePurchaseCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock IAP.MakePurchaseAsync({purchaseConfig})");
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

        public void ConsumePurchaseAsync(string token, Action onSuccess, Action<WortalError> onError)
        {
            _consumePurchaseCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPConsumePurchaseJS(token, IAPConsumePurchaseCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock IAP.ConsumePurchaseAsync({token})");
            IAPConsumePurchaseCallback();
#endif
        }

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

        #endregion JSlib Interface

        #region Callback Methods

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void IAPGetCatalogCallback(string catalog)
        {
            Product[] catalogObj;

            try
            {
                catalogObj = JsonConvert.DeserializeObject<Product[]>(catalog);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "IAPGetCatalogCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getCatalogCallback?.Invoke(catalogObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void IAPGetPurchasesCallback(string purchases)
        {
            Purchase[] purchasesObj;

            try
            {
                purchasesObj = JsonConvert.DeserializeObject<Purchase[]>(purchases);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "IAPGetPurchasesCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getPurchasesCallback?.Invoke(purchasesObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void IAPMakePurchaseCallback(string purchase)
        {
            Purchase purchaseObj;

            try
            {
                purchaseObj = JsonConvert.DeserializeObject<Purchase>(purchase);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "IAPMakePurchaseCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _makePurchaseCallback?.Invoke(purchaseObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void IAPConsumePurchaseCallback()
        {
            _consumePurchaseCallback?.Invoke();
        }

        #endregion Callback Methods
    }
}
