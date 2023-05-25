#if UNITY_LOCALIZATION
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for a ContextPayload to be used in social features.
    /// </summary>
    [CreateAssetMenu(fileName = "ContextPayloadTemplate", menuName = "Wortal/Context Payload Template")]
    public class ContextPayloadTemplate : ScriptableObject
    {
#region Private Members

        [Tooltip("URL of base64 encoded image to be displayed. This is required for the payload to be sent.")]
        [SerializeField]
        private Texture2D _image;
        [Tooltip("Message body. This is required for the payload to be sent.")]
        [SerializeField]
        private LocalizedString _text;
        [Tooltip("Text of the call-to-action button.")]
        [SerializeField]
        private LocalizedString _cta;
        [Tooltip("An array of filters to be applied to the friend list. Only the first filter is currently used.")]
        [SerializeField]
        private WortalContext.ContextFilter[] _filters;
        [Tooltip("Context minimum size.")]
        [SerializeField]
        private int _minSize;
        [Tooltip("Context maximum size.")]
        [SerializeField]
        private int _maxSize;
        [Tooltip("Specify how long a friend should be filtered out after the current player sends them a message. This parameter only applies when `NEW_INVITATIONS_ONLY` filter is used. When not specified, it will filter out any friend who has been sent a message.")]
        [SerializeField]
        private int _hoursSinceInvitation;
        [Tooltip("Optional customizable text field in the share UI. This can be used to describe the incentive a user can get from sharing.")]
        [SerializeField]
        private LocalizedString _description;
        [Tooltip("Message format to be used. There's no visible difference among the available options.")]
        [SerializeField]
        private WortalContext.IntentType _intent;
        [Tooltip("Optional property to switch share UI mode.")]
        [SerializeField]
        private WortalContext.UIType _ui;
        [Tooltip("Defines the minimum number of players to be selected to start sharing.")]
        [SerializeField]
        private int _minShare;
        [Tooltip("Defines how the update message should be delivered.")]
        [SerializeField]
        private WortalContext.StrategyType _strategy;
        [Tooltip("Specifies if the message should trigger push notification.")]
        [SerializeField]
        private WortalContext.NotificationsType _notifications;
        [Tooltip("Specifies where the share should appear.")]
        [SerializeField]
        private WortalContext.ShareDestination _shareDestination;
        [Tooltip("Should the player switch context or not.")]
        [SerializeField]
        private bool _switchContext;
        [Tooltip("Not used.")]
        private string _action;
        [Tooltip("Not used.")]
        private string _template;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Context API calls. This will convert the image
        /// to a base64 string and get the localized strings for every language supported by the game.
        /// </summary>
        /// <returns></returns>
        public ContextPayload GetPayload()
        {
            if (_image == null)
            {
                throw new Exception("[ContextPayload] Image is required for the payload to be sent.");
            }

            if (_text.IsEmpty)
            {
                throw new Exception("[ContextPayload] Text is required for the payload to be sent.");
            }

            ContextPayload payload = new ContextPayload();

            // These 2 are required for the payload to be sent. The SDK will throw an error if they are not set.
            payload.Image = "data:image/png;base64," + Convert.ToBase64String(_image.EncodeToPNG());
            payload.Text = new LocalizableContent
            {
                Default = _text.GetLocalizedString(),
                Localizations = GetLocalizationDictionary(_text),
            };

            // Defaults to "Play" or something like that if not set.
            if (!_cta.IsEmpty)
            {
                payload.CTA = new LocalizableContent
                {
                    Default = _cta.GetLocalizedString(),
                    Localizations = GetLocalizationDictionary(_cta),
                };
            }

            payload.Filters = _filters;
            payload.MinSize = _minSize;
            payload.MaxSize = _maxSize;
            payload.HoursSinceInvitation = _hoursSinceInvitation;

            if (!_description.IsEmpty)
            {
                payload.Description = new LocalizableContent
                {
                    Default = _description.GetLocalizedString(),
                    Localizations = GetLocalizationDictionary(_description),
                };
            }

            payload.Intent = _intent;
            payload.UI = _ui;
            payload.MinShare = _minShare;
            payload.Strategy = _strategy;
            payload.Notifications = _notifications;
            payload.ShareDestination = _shareDestination;
            payload.SwitchContext = _switchContext;
            payload.Action = _action;
            payload.Template = _template;

            return payload;
        }

#endregion Public API
#region Private Methods

        private static Dictionary<string, string> GetLocalizationDictionary(LocalizedString localizedString)
        {
            Dictionary<string, string> localizationDictionary = new();
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                localizationDictionary.Add(locale.Identifier.Code, localizedString.GetLocalizedString(locale.Identifier.Code));
            }

            return localizationDictionary;
        }

#endregion Private Methods
    }
}
#endif
