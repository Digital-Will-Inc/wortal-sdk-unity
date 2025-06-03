// Runtime/Core/Data/PlayerStats.cs
using System;
using System.Collections.Generic;

namespace DigitalWill.WortalSDK.Core
{
    [Serializable]
    public class PlayerStats
    {
        public Dictionary<string, object> stats = new Dictionary<string, object>();
    }
}
