using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
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
}
