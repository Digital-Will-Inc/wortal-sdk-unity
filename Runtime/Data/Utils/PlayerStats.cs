using System;
using System.Collections.Generic;

namespace DigitalWill.WortalSDK
{
    [Serializable]
    public class PlayerStats
    {
        public Dictionary<string, object> stats = new Dictionary<string, object>();

        public void SetStat(string key, object value)
        {
            stats[key] = value;
        }
    }
}
