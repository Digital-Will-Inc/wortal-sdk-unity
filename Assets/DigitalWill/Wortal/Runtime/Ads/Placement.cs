namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Types of ad placements as defined by Google:
    /// https://developers.google.com/ad-placement/docs/placement-types
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// The player navigates to the next level.
        /// </summary>
        Next,
        /// <summary>
        /// Your game has loaded, the UI is visible and sound is enabled, the player can interact with the game,
        /// but the game play has not started yet.
        /// </summary>
        Start,
        /// <summary>
        /// The player pauses the game.
        /// </summary>
        Pause,
        /// <summary>
        /// The player explores options outside of gameplay.
        /// </summary>
        Browse,
    }
}
