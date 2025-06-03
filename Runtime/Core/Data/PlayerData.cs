// Runtime/Core/Data/PlayerData.cs
using System;
using System.Collections.Generic;

namespace DigitalWill.WortalSDK.Core
{
    [Serializable]
    public class PlayerData
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();
    }
}
