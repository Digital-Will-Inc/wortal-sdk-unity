// ReSharper disable CheckNamespace
using DigitalWill.Core;
using UnityEditor;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Custom editor window for managing the Soup package. Soup settings should only be modified in this window to
    /// avoid conflicts.
    /// </summary>
    public class SoupEditorWindow : ExtendedEditorWindow
    {
        private const int WIDTH = 760;
        private const int HEIGHT = 400;
        private const string TITLE = "Wortal Soup Settings";

        private string _version;
        private string _repoLink;

        private Texture2D _logo;

        private void Awake()
        {
            _version = SoupComponent.SoupSettings.PACKAGE_VERSION;
            _repoLink = SoupComponent.SoupSettings.PACKAGE_REPO;
            _logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/DigitalWill/Gizmos/Soup.png");
        }

        [MenuItem("DigitalWill/Soup Settings")]
        public static void ShowSoupWindow()
        {
            var window = GetWindow<SoupEditorWindow>();
            window._serializedObject = new SerializedObject(SoupComponent.SoupSettings);
            window.titleContent = new GUIContent(TITLE);
            window.minSize = new Vector2(WIDTH, HEIGHT);
        }

        private void OnGUI()
        {
            _serializedObject.Update();

            GUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope("box"))
            {
                DrawSoupHeader();
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                DrawSoupInfo();
                GUILayout.Space(5);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            GUILayout.Space(15);
            DrawServices();
            GUILayout.Space(30);
            DrawConfig();
            GUILayout.Space(15);
            GUILayout.FlexibleSpace();
            GUILayout.Label(_logo);

            _serializedObject.ApplyModifiedProperties();
        }

        private void DrawSoupHeader()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                EditorGUILayout.LabelField("Version", new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 13,
                    fixedHeight = 20,
                    stretchWidth = true,
                    fixedWidth = WIDTH / 4,
                    clipping = TextClipping.Overflow,
                    padding = new RectOffset(WIDTH / 4 + 15, 0, 0, 0)
                });

                GUILayout.Space(85);

                EditorGUILayout.LabelField("Repo", new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 13,
                    fixedHeight = 20,
                    stretchWidth = true,
                    fixedWidth = Screen.width / 4,
                    clipping = TextClipping.Overflow,
                });
            }
        }

        private void DrawSoupInfo()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                EditorGUILayout.LabelField(_version, new GUIStyle(EditorStyles.label)
                {
                    fontSize = 13,
                    fixedHeight = 20,
                    stretchWidth = true,
                    fixedWidth = WIDTH / 4,
                    clipping = TextClipping.Overflow,
                    padding = new RectOffset(WIDTH / 4 + 25, 0, 0, 0)
                });

                if (
                GUILayout.Button(new GUIContent
                {
                    text = "GitHub",
                    tooltip = "Check out the Soup repo on GitHub.",
                }, GUILayout.Width(90)))
                {
                    Application.OpenURL(_repoLink);
                }

                GUILayout.Space(260);
            }
        }

        private void DrawServices()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Space(25);

                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(false)))
                {
                    EditorGUILayout.LabelField("Services", new GUIStyle(EditorStyles.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fontSize = 13,
                        fixedHeight = 20,
                        stretchWidth = true,
                        fixedWidth = Screen.width / 4,
                        clipping = TextClipping.Overflow,
                    });

                    DrawField("Audio", true);
                    DrawField("LocalData", true);
                    DrawField("Localization", true);
                }

                GUILayout.Space(50);
            }
        }

        private void DrawConfig()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Space(25);

                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(false)))
                {
                    EditorGUILayout.LabelField("Soup Localization", new GUIStyle(EditorStyles.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fontSize = 13,
                        fixedHeight = 20,
                        stretchWidth = true,
                        fixedWidth = Screen.width / 4,
                        clipping = TextClipping.Overflow,
                    });

                    DrawField("DefaultLanguage", true);
                    DrawField("DefaultFont", true);
                    DrawField("UseFontAtlasBuilder", true);
                    DrawField("CustomFonts", true);
                }

                GUILayout.Space(25);

                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(false)))
                {
                    EditorGUILayout.LabelField("Soup Logging", new GUIStyle(EditorStyles.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fontSize = 13,
                        fixedHeight = 20,
                        stretchWidth = true,
                        fixedWidth = Screen.width / 4,
                        clipping = TextClipping.Overflow,
                    });

                    DrawField("LogLevel", true);
                    DrawField("SaveLogsToFile", true);
                }

                GUILayout.Space(25);
            }
        }
    }
}
