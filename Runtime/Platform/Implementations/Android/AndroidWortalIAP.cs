using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalIAP : IWortalIAP
    {
        public bool IsSupported => false;

        public void GetCatalogAsync(Action<Product[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalIAP.GetCatalogAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetCatalogAsync implementation"
            });
        }

        public void GetPurchasesAsync(Action<Purchase[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalIAP.GetPurchasesAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetPurchasesAsync implementation"
            });
        }

        public void MakePurchaseAsync(PurchaseConfig purchaseConfig, Action<Purchase> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalIAP.MakePurchaseAsync({purchaseConfig}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "MakePurchaseAsync implementation"
            });
        }

        public void ConsumePurchaseAsync(string token, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalIAP.ConsumePurchaseAsync({token}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "ConsumePurchaseAsync implementation"
            });
        }
    }
}