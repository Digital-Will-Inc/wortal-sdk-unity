// Runtime/Core/Data/SignedPlayerInfo.cs
using System;

namespace DigitalWill.WortalSDK.Core
{
    [Serializable]
    public class SignedPlayerInfo
    {
        public string playerID;
        public string signature;
        public object requestPayload;
    }
}
