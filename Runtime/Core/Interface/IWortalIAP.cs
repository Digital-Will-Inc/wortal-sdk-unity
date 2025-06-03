using System;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Interface for In-App Purchase functionality across platforms
    /// </summary>
    public interface IWortalIAP
    {
        /// <summary>
        /// Gets the catalog of available products
        /// </summary>
        /// <param name="onSuccess">Callback with product catalog</param>
        /// <param name="onError">Callback for catalog errors</param>
        void GetCatalogAsync(Action<Product[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the player's purchase history
        /// </summary>
        /// <param name="onSuccess">Callback with purchase history</param>
        /// <param name="onError">Callback for history errors</param>
        void GetPurchasesAsync(Action<Purchase[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Makes a purchase
        /// </summary>
        /// <param name="purchaseConfig">Purchase configuration</param>
        /// <param name="onSuccess">Callback for successful purchase</param>
        /// <param name="onError">Callback for purchase errors</param>
        void MakePurchaseAsync(PurchaseConfig purchaseConfig, Action<Purchase> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Consumes a purchase
        /// </summary>
        /// <param name="token">Purchase token to consume</param>
        /// <param name="onSuccess">Callback for successful consumption</param>
        /// <param name="onError">Callback for consumption errors</param>
        void ConsumePurchaseAsync(string token, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if IAP is supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
