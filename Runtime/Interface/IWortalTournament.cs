using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Interface for tournament functionality across platforms
    /// </summary>
    public interface IWortalTournament
    {
        /// <summary>
        /// Fetch the tournament out of the current context the user is playing.
        /// </summary>
        /// <param name="callback">Callback with the current tournament. Fired when the async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void GetCurrent(Action<Tournament> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Returns a list of eligible tournaments that can be surfaced in-game.
        /// </summary>
        /// <param name="callback">Callback with an array of active tournaments. Fired when the async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void GetAll(Action<Tournament[]> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Posts a player's score. This API should only be called within a tournament context at the end of an activity.
        /// </summary>
        /// <param name="score">An integer value representing the player's score at the end of an activity.</param>
        /// <param name="callback">Void callback event triggered when the async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void PostScore(int score, Action callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Opens the tournament creation dialog if the player is not currently in a tournament session.
        /// </summary>
        /// <param name="payload">Payload that defines the tournament configuration.</param>
        /// <param name="callback">Callback that contains the created tournament. Fired when the async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void Create(CreateTournamentPayload payload, Action<Tournament> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Opens the share tournament dialog if the player is currently in a tournament session.
        /// </summary>
        /// <param name="payload">Specifies share content.</param>
        /// <param name="callback">Void callback event triggered when the async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void Share(ShareTournamentPayload payload, Action callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Request a switch into a specific tournament context.
        /// </summary>
        /// <param name="tournamentID">ID of the desired tournament context to switch into.</param>
        /// <param name="callback">Void callback event triggered when the async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void Join(string tournamentID, Action callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Checks if tournament features are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}