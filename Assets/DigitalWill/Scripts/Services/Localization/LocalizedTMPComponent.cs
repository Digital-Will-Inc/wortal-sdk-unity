using TMPro;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Localizes text elements based on the <see cref="LanguageCode"/> set in <see cref="ILocalizationService"/>.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMPComponent : MonoBehaviour
    {
        [Header("Localization Properties")]
        [Tooltip("Key string of the text to be displayed from the language file. Should start with @.")]
        [SerializeField] private string _key;
        [Tooltip("Optional parameters to pass with the value. These show up as {O}, {1}, etc. in the language file and will be displayed in order.")]
        [SerializeField] private string[] _params;

        [Header("Game Data Calls")]
        [Tooltip("Gets the current level to use in parameters. Use the parameter CURRENT_LEVEL where you want the actual level data to be replaced.")]
        [SerializeField] private bool _needCurrentLevel;
        [Tooltip("Gets the current score to use in parameters. Use the parameter CURRENT_SCORE where you want the actual score data to be replaced.")]
        [SerializeField] private bool _needCurrentScore;

        private const string LEVEL_STRING = "CURRENT_LEVEL";
        private const string SCORE_STRING = "CURRENT_SCORE";

        private ILocalizationService _localization;
        private TMP_Text _textField;

        private void Awake()
        {
            _localization = GameServices.I.Get<ILocalizationService>();
            _textField = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _localization.LanguageChanged += OnLanguageChanged;
            _localization.FontChanged += OnFontChanged;
            _textField.font = _localization.Font;

            CheckForParamsData(_needCurrentLevel, _needCurrentScore);
            SetLocalizedText(_textField);
        }

        private void OnDestroy()
        {
            _localization.LanguageChanged -= OnLanguageChanged;
            _localization.FontChanged -= OnFontChanged;
        }

        /// <summary>
        /// Changes the key of the localization string.
        /// </summary>
        /// <param name="key">Key of the new string to be localized.</param>
        public void SetKey(string key)
        {
            _key = key;
            SetLocalizedText(_textField);
        }

        private void OnLanguageChanged(LanguageCode languageCode)
        {
            SetLocalizedText(_textField);
        }

        private void OnFontChanged(LanguageCode languageCode)
        {
            _textField.font = _localization.Font;
        }

        private void SetLocalizedText(TMP_Text text)
        {
            string value = _localization.GetValue(_key, _params);
            text.text = value;
        }

        private void CheckForParamsData(bool currentLevel, bool currentScore)
        {
            if (currentLevel)
            {
                var level = GameServices.I.Get<ILocalDataService>().CurrentLevel.ToString();
                ReplaceParamsWithData(LEVEL_STRING, level);
            }

            if (currentScore)
            {
                var score = GameServices.I.Get<ILocalDataService>().CurrentScore.ToString();
                ReplaceParamsWithData(SCORE_STRING, score);
            }
        }

        private void ReplaceParamsWithData(string paramConst, string replacement)
        {
            for (int i = 0; i < _params.Length; i++)
            {
                if (_params[i] == paramConst)
                {
                    _params[i] = replacement;
                }
            }
        }
    }
}
