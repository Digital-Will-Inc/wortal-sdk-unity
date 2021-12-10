using UnityEngine;

namespace DigitalWill.Util
{
    /// <summary>
    /// Static helper class that pools yields to avoid garbage collection being triggered.
    /// </summary>
    public static class YieldHelper
    {
        public static readonly WaitForEndOfFrame END_OF_FRAME = new WaitForEndOfFrame();
        public static readonly WaitForSeconds TENTH_SECOND = new WaitForSeconds(0.1f);
        public static readonly WaitForSeconds HALF_SECOND = new WaitForSeconds(0.5f);
        public static readonly WaitForSeconds ONE_SECOND = new WaitForSeconds(1.0f);
        public static readonly WaitForSeconds TWO_SECONDS = new WaitForSeconds(2.0f);
        public static readonly WaitForSeconds FIVE_SECONDS = new WaitForSeconds(5.0f);
    }
}
