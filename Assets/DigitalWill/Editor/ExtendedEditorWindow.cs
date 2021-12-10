// ReSharper disable CheckNamespace
using UnityEditor;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Extends the editor window to provide some additional features for customized windows.
    /// </summary>
    /// <remarks>Borrowed from: https://www.youtube.com/watch?v=c_3DXBrH-Is</remarks>
    public class ExtendedEditorWindow : EditorWindow
    {
        protected SerializedObject _serializedObject;
        protected SerializedProperty _currentProperty;

        protected SerializedProperty _selectedProperty;
        private string _selectedPropertyPath;

        /// <summary>
        /// Draws the properties associated with the SerializedProperty given.
        /// </summary>
        /// <param name="property">Property to draw.</param>
        /// <param name="drawChildren">Should we draw the children of the property or not.</param>
        protected void DrawProperties(SerializedProperty property, bool drawChildren)
        {
            var lastPropertyPath = string.Empty;

            foreach (SerializedProperty p in property)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropertyPath) && p.propertyPath.Contains(lastPropertyPath))
                    {
                        continue;
                    }

                    lastPropertyPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        /// <summary>
        /// Draws a property field.
        /// </summary>
        /// <param name="propertyName">Name of the property to be drawn.</param>
        /// <param name="relative">Get a property at a path relative to the current property.</param>
        protected void DrawField(string propertyName, bool relative)
        {
            if (relative && _currentProperty != null)
            {
                EditorGUILayout.PropertyField(_currentProperty.FindPropertyRelative(propertyName), true);
            }
            else if (_serializedObject != null)
            {
                EditorGUILayout.PropertyField(_serializedObject.FindProperty(propertyName), true);
            }
        }

        /// <summary>
        /// Draws a property field with a custom label to override the property name.
        /// </summary>
        /// <param name="propertyName">Name of the property to be drawn.</param>
        /// <param name="labelName">Label to be displayed for this property.</param>
        /// <param name="relative">Get a property at a path relative to the current property.</param>
        protected void DrawField(string propertyName, string labelName, bool relative)
        {
            if (relative && _currentProperty != null)
            {
                EditorGUILayout.PropertyField(_currentProperty.FindPropertyRelative(propertyName), new GUIContent(labelName), true);
            }
            else if (_serializedObject != null)
            {
                EditorGUILayout.PropertyField(_serializedObject.FindProperty(propertyName), new GUIContent(labelName), true);
            }
        }

        /// <summary>
        /// Draws a property field with a custom label and tooltip.
        /// </summary>
        /// <param name="propertyName">Name of the property to be drawn.</param>
        /// <param name="labelName">Label to be displayed for this property.</param>
        /// <param name="tooltip">Tooltip to be displayed for this property.</param>
        /// <param name="relative">Get a property at a path relative to the current property.</param>
        protected void DrawField(string propertyName, string labelName, string tooltip, bool relative)
        {
            if (relative && _currentProperty != null)
            {
                EditorGUILayout.PropertyField(_currentProperty.FindPropertyRelative(propertyName), new GUIContent(labelName, tooltip), true);
            }
            else if (_serializedObject != null)
            {
                EditorGUILayout.PropertyField(_serializedObject.FindProperty(propertyName), new GUIContent(labelName, tooltip), true);
            }
        }

        /// <summary>
        /// Draws a sidebar to select properties from.
        /// </summary>
        /// <param name="property">Property used to populate the sidebar.</param>
        /// <remarks>Useful if you have several custom classes or ScriptableObjects you want to work with, such as
        /// character details. Allows designer to focus on one object at a time.</remarks>
        protected void DrawSidebar(SerializedProperty property)
        {
            foreach (SerializedProperty p in property)
            {
                if (GUILayout.Button(p.displayName))
                {
                    _selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(_selectedPropertyPath))
            {
                _selectedProperty = _serializedObject.FindProperty(_selectedPropertyPath);
            }
        }
    }
}
