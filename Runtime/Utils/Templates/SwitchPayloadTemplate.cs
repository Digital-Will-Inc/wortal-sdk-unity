using UnityEngine;
using UnityEngine.Localization;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for a SwitchPayload to be used in social features.
    /// </summary>
    [CreateAssetMenu(fileName = "SwitchPayloadTemplate", menuName = "Wortal/Switch Payload Template")]
    public class SwitchPayloadTemplate : ScriptableObject
    {
#region Private Members
        [Tooltip("Message which will be displayed to contact. If not specified, \"SENDER_NAMEと一緒に「GAME_NAME」をプレイしよう！\" will be used by default.")]
        [SerializeField]
        private LocalizedString _text;
        [Tooltip("Text of the call-to-action button. If not specified, \"今すぐプレイ\" will be used by default.")]
        [SerializeField]
        private LocalizedString _caption;
        [Tooltip("If switching into a solo context, set this to true to switch silently, with no confirmation dialog or toast. This only has an effect when switching into a solo context. Defaults to false.")]
        [SerializeField]
        private bool _switchSilentlyIfSolo;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Context API calls.
        /// </summary>
        /// <returns>Payload to be passed into Context API.</returns>
        public SwitchPayload GetPayload()
        {
            SwitchPayload payload = new SwitchPayload();

            if (!_text.IsEmpty)
            {
                payload.Text = new LocalizableContent
                {
                    Default = _text.GetLocalizedString(),
                    Localizations = TemplateUtils.GetLocalizationDictionary(_text),
                };
            }

            if (!_caption.IsEmpty)
            {
                payload.Caption = new LocalizableContent
                {
                    Default = _caption.GetLocalizedString(),
                    Localizations = TemplateUtils.GetLocalizationDictionary(_caption),
                };
            }

            payload.SwitchSilentlyIfSolo = _switchSilentlyIfSolo;

            return payload;
        }

#endregion Public API
    }
}
