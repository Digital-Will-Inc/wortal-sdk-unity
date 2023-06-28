#if UNITY_LOCALIZATION
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for an InvitePayload to be used in social features.
    /// </summary>
    [CreateAssetMenu(fileName = "InvitePayloadTemplate", menuName = "Wortal/Invite Payload Template")]
    public class InvitePayloadTemplate : ScriptableObject
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
        [Tooltip("The set of filters to apply to the suggestions. Multiple filters may be applied. If no results are returned when the filters are applied, the results will be generated without the filters.")]
        [SerializeField]
        private InviteFilter[] _filters;
        [Tooltip("An optional title to display at the top of the invite dialog instead of the generic title. This param is not sent as part of the message, but only displays in the dialog header. The title can be either a string or an object with the default text as the value of 'default' and another object mapping locale keys to translations as the value of 'localizations'.")]
        [SerializeField]
        private LocalizedString _dialogTitle;
        [Tooltip("The set of sections to be included in the dialog. Each section can be assigned a maximum number of results to be returned (up to a maximum of 10). If no max is included, a default max will be applied. Sections will be included in the order they are listed in the array. The last section will include a larger maximum number of results, and if a maxResults is provided, it will be ignored. If this array is left empty, default sections will be used.")]
        [SerializeField]
        private InviteSection[] _sections;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Context API calls.
        /// </summary>
        /// <returns>Payload to be passed into Context API.</returns>
        /// <exception cref="MissingMemberException">Thrown if any required properties are missing or null.</exception>
        public InvitePayload GetPayload()
        {
            if (_image == null)
            {
                throw new MissingMemberException("[Wortal] Image is required for the invite payload to be sent.");
            }

            if (_text.IsEmpty)
            {
                throw new MissingMemberException("[Wortal] Text is required for the invite payload to be sent.");
            }

            var payload = new InvitePayload();

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

            if (!_dialogTitle.IsEmpty)
            {
                payload.DialogTitle = new LocalizableContent
                {
                    Default = _dialogTitle.GetLocalizedString(),
                    Localizations = TemplateUtils.GetLocalizationDictionary(_dialogTitle),
                };
            }

            payload.Filters = _filters;
            payload.Sections = _sections;

            return payload;
        }

#endregion Public API
    }
}
#endif
