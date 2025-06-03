using System;

namespace DigitalWill.WortalSDK.Core
{
    [Serializable]
    public class ContextChoosePayload
    {
        public ContextFilter[] filters;
        public int maxSize;
        public int minSize;
    }
}
