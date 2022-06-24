namespace DigitalWill.H5Portal
{
    /// <summary>
    /// Types of ad placements.
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// Used when ad is shown before next level, restart, revive, etc.
        /// </summary>
        Next,
        /// <summary>
        /// Used only at the very beginning when the game is first loading. For preroll ads.
        /// </summary>
        Start,
        /// <summary>
        /// Used when an ad is shown after the player pauses the game.
        /// </summary>
        Pause,
        /// <summary>
        /// Used when an ad is shown outside of gameplay. Such as a player browser character skin options, etc.
        /// </summary>
        Browse,
    }
}
