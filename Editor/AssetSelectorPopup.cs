using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalEditor
{
    public class AssetSelectorPopup : EditorWindow
    {
        private DigitalWill.WortalSDK.AssetPriorityGroup group;
        private Vector2 scroll;
        private List<Object> selectedAssets = new();
        private LazyLoadConfig config;

        public static void Show(DigitalWill.WortalSDK.LazyLoadConfig config, DigitalWill.WortalSDK.AssetPriorityGroup group)
        {
            var window = CreateInstance<AssetSelectorPopup>();
            window.group = group;
            window.config = config;
            window.titleContent = new GUIContent("Select Assets for " + group.name);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 400);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField($"Editing Assets for Group: {group.name}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var kvp in group.assetsByType)
            {
                string type = kvp.Key;
                List<string> paths = kvp.Value;

                EditorGUILayout.LabelField($"Type: {type}", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                for (int i = 0; i < paths.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    Object asset = AssetDatabase.LoadAssetAtPath<Object>(paths[i]);
                    asset = EditorGUILayout.ObjectField(asset, typeof(Object), false);

                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        paths.RemoveAt(i);
                        GUIUtility.ExitGUI();
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add New Asset", EditorStyles.boldLabel);

            Object newAsset = EditorGUILayout.ObjectField(null, typeof(Object), false);
            if (newAsset != null)
            {
                string path = AssetDatabase.GetAssetPath(newAsset);
                var type = AssetDatabase.GetMainAssetTypeAtPath(path);
                string typeName = type != null ? type.Name : "Unknown";

                if (!group.assetsByType.ContainsKey(typeName))
                    group.assetsByType[typeName] = new List<string>();

                if (!group.assetsByType[typeName].Contains(path))
                {
                    group.assetsByType[typeName].Add(path);
                }
            }

            if (GUILayout.Button("Done"))
            {
                group.UpdateCountAndSize();
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                Close();
            }
        }
    }
}
