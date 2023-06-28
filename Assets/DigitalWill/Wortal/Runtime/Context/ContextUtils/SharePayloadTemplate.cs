#if UNITY_LOCALIZATION
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for a SharePayload to be used in social features.
    /// </summary>
    [CreateAssetMenu(fileName = "SharePayloadTemplate", menuName = "Wortal/Share Payload Template")]
    public class SharePayloadTemplate : ScriptableObject
    {
#region Private Members

        [Header("Required Properties")]
        [Tooltip("Data URL of base64 encoded image to be displayed. This is required for the payload to be sent.")]
        [SerializeField]
        private Texture2D _image;
        [Tooltip("A text message, or an object with the default text as the value of 'default' and another object mapping locale keys to translations as the value of 'localizations'.")]
        [SerializeField]
        private LocalizedString _text;

        [Header("Optional Properties")]
        [Tooltip("Text of the call-to-action button.")]
        [SerializeField]
        private LocalizedString _cta;
        [Tooltip("Optional customizable text field in the share UI. This can be used to describe the incentive a user can get from sharing.")]
        [SerializeField]
        private LocalizedString _description;
        [Tooltip("An array of filters to be applied to the friend list. Only the first filter is currently used.")]
        [SerializeField]
        private ContextFilter[] _filters;
        [Tooltip("Specify how long a friend should be filtered out after the current player sends them a message. This parameter only applies when `NEW_INVITATIONS_ONLY` filter is used. When not specified, it will filter out any friend who has been sent a message.")]
        [SerializeField]
        private int _hoursSinceInvitation;
        [Tooltip("Message format to be used. There's no visible difference among the available options.")]
        [SerializeField]
        private IntentType _intent;
        [Tooltip("Defines the minimum number of players to be selected to start sharing.")]
        [SerializeField]
        private int _minShare;
        [Tooltip("Optional property to switch share UI mode.")]
        [SerializeField]
        private UIType _ui;
        [Tooltip("An optional array to set sharing destinations in the share dialog. If not specified all available sharing destinations will be displayed.")]
        [SerializeField]
        private ShareDestination[] _shareDestination;
        [Tooltip("Should the player switch context or not.")]
        [SerializeField]
        private bool _switchContext;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Context API calls.
        /// </summary>
        /// <returns>Payload to be passed into Context API.</returns>
        /// <exception cref="MissingMemberException">Thrown if any required properties are missing or null.</exception>
        public SharePayload GetPayload()
        {
            if (_image == null)
            {
                throw new MissingMemberException("[Wortal] Image is required for the share payload to be sent.");
            }

            if (_text.IsEmpty)
            {
                throw new MissingMemberException("[Wortal] Text is required for the share payload to be sent.");
            }

            var payload = new SharePayload();

            // These 2 are required for the payload to be sent. The SDK will throw an error if they are not set.
            payload.Image = "data:image/png;base64," + Convert.ToBase64String(_image.EncodeToPNG());
            payload.Text = new LocalizableContent
            {
                Default = _text.GetLocalizedString(),
                Localizations = TemplateUtils.GetLocalizationDictionary(_text),
            };

            // Defaults to "Play" or something like that if not set.
            if (!_cta.IsEmpty)
            {
                payload.CTA = new LocalizableContent
                {
                    Default = _cta.GetLocalizedString(),
                    Localizations = TemplateUtils.GetLocalizationDictionary(_cta),
                };
            }

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
            payload.Intent = _intent;
            payload.UI = _ui;
            payload.MinShare = _minShare;
            payload.ShareDestination = _shareDestination;
            payload.SwitchContext = _switchContext;

            return payload;
        }

#endregion Public API
    }
}
#endif
