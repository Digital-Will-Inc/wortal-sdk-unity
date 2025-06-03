using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Interface for context functionality across platforms
    /// </summary>
    public interface IWortalContext
    {
        /// <summary>
        /// Gets the current context ID
        /// </summary>
        /// <returns>Context ID or null if not in a context</returns>
        string GetID();

        /// <summary>
        /// Gets the current context type
        /// </summary>
        /// <returns>Context type</returns>
        ContextType GetType();

        /// <summary>
        /// Gets players in the current context
        /// </summary>
        /// <param name="onSuccess">Callback with context players</param>
        /// <param name="onError">Callback for errors</param>
        void GetPlayersAsync(Action<ContextPlayer[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Invites players to the game
        /// </summary>
        /// <param name="payload">Invite payload</param>
        /// <param name="onSuccess">Callback for successful invite</param>
        /// <param name="onError">Callback for invite errors</param>
        void InviteAsync(InvitePayload payload, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Shares content with the context
        /// </summary>
        /// <param name="payload">Share payload</param>
        /// <param name="onSuccess">Callback for successful share</param>
        /// <param name="onError">Callback for share errors</param>
        void ShareAsync(SharePayload payload, Action<bool> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Shares a link with the context
        /// </summary>
        /// <param name="payload">Link share payload</param>
        /// <param name="onSuccess">Callback for successful link share</param>
        /// <param name="onError">Callback for link share errors</param>
        void ShareLinkAsync(LinkSharePayload payload, Action<bool> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Updates the context with new content
        /// </summary>
        /// <param name="payload">Update payload</param>
        /// <param name="onSuccess">Callback for successful update</param>
        /// <param name="onError">Callback for update errors</param>
        void UpdateAsync(UpdatePayload payload, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Switches to a different context
        /// </summary>
        /// <param name="contextID">ID of the context to switch to</param>
        /// <param name="onSuccess">Callback for successful context switch</param>
        /// <param name="onError">Callback for context switch errors</param>
        void SwitchAsync(string contextID, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Chooses a context from available options
        /// </summary>
        /// <param name="options">Context choice options</param>
        /// <param name="onSuccess">Callback for successful context choice</param>
        /// <param name="onError">Callback for context choice errors</param>
        void ChooseAsync(ContextChoosePayload options, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <param name="playerID">ID of the player to create context with</param>
        /// <param name="onSuccess">Callback for successful context creation</param>
        /// <param name="onError">Callback for context creation errors</param>
        void CreateAsync(string playerID, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if context features are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
