#if UNITY_LOCALIZATION
using UnityEngine;
using UnityEngine.Localization;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for a ChoosePayload to be used in social features.
    /// </summary>
    [CreateAssetMenu(fileName = "ChoosePayloadTemplate", menuName = "Wortal/Choose Payload Template")]
    public class ChoosePayloadTemplate : ScriptableObject
    {
#region Private Members

        [Header("Optional Properties")]
        [Tooltip("Optional customizable text field in the share UI. This can be used to describe the incentive a user can get from sharing.")]
        [SerializeField]
        private LocalizedString _description;
        [Tooltip("An array of filters to be applied to the friend list. Only the first filter is currently used.")]
        [SerializeField]
        private ContextFilter[] _filters;
        [Tooltip("Specify how long a friend should be filtered out after the current player sends them a message. This parameter only applies when `NEW_INVITATIONS_ONLY` filter is used. When not specified, it will filter out any friend who has been sent a message.")]
        [SerializeField]
        private int _hoursSinceInvitation;
        [Tooltip("Context minimum size.")]
        [SerializeField]
        private int _minSize;
        [Tooltip("Context maximum size.")]
        [SerializeField]
        private int _maxSize;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Context API calls.
        /// </summary>
        /// <returns>Payload to be passed into Context API.</returns>
        public ChoosePayload GetPayload()
        {
            ChoosePayload payload = new ChoosePayload();

            if (!_description.IsEmpty)
            {
                payload.Description = new LocalizableContent
                {
                    Default = _description.GetLocalizedString(),
                    Localizations = TemplateUtils.GetLocalizationDictionary(_description),
                };
            }

            payload.Filters = _filters;
            payload.HoursSinceInvitation = _hoursSinceInvitation;
            payload.MinSize = _minSize;
            payload.MaxSize = _maxSize;

            return payload;
        }

#endregion Public API
    }
}
#endif
