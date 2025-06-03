using System;

namespace DigitalWill.WortalSDK.Core
{
    [Serializable]
    public class AuthResponse
    {
        public string playerID;
        public string playerName;
        public string playerPhoto;
        public bool isFirstTime;
        public object platformData;
    }
}
