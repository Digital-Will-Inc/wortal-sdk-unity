#if UNITY_LOCALIZATION
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for an UpdatePayload to be used in social features.
    /// </summary>
    [CreateAssetMenu(fileName = "UpdatePayloadTemplate", menuName = "Wortal/Update Payload Template")]
    public class UpdatePayloadTemplate : ScriptableObject
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
        [Tooltip("Optional content for a gif or video. At least one image or media should be provided in order to render the update.")]
        [SerializeField]
        private MediaParams _media;
        [Tooltip("Specifies notification setting for the custom update. This can be 'NO_PUSH' or 'PUSH', and defaults to 'NO_PUSH'. Use push notification only for updates that are high-signal and immediately actionable for the recipients. Also note that push notification is not always guaranteed, depending on user setting and platform policies.")]
        [SerializeField]
        private NotificationsType _notifications;
        [Tooltip("Defines how the update message should be delivered.")]
        [SerializeField]
        private StrategyType _strategy;
        [Tooltip("Message format to be used. Not currently used.")]
        private string _action;
        [Tooltip("ID of the template this custom update is using. Templates should be predefined in fbapp-config.json. See the [Bundle Config documentation](https://developers.facebook.com/docs/games/instant-games/bundle-config) for documentation about fbapp-config.json.")]
        private string _template;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Context API calls.
        /// </summary>
        /// <returns>Payload to be passed into Context API.</returns>
        /// <exception cref="MissingMemberException">Thrown if any required properties are missing or null.</exception>
        public UpdatePayload GetPayload()
        {
            if (_image == null)
            {
                throw new MissingMemberException("[Wortal] Image is required for the share payload to be sent.");
            }

            if (_text.IsEmpty)
            {
                throw new MissingMemberException("[Wortal] Text is required for the share payload to be sent.");
            }

            var payload = new UpdatePayload();

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

            payload.Media = _media;
            payload.Notifications = _notifications;
            payload.Strategy = _strategy;
            payload.Action = _action;
            payload.Template = _template;

            return payload;
        }

#endregion Public API
    }
}
#endif
