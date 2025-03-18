using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;
using System.Linq;

namespace FTKingdom
{
    [CustomPropertyDrawer(typeof(DialogueLine))]
    public class DialogueLineDrawer : PropertyDrawer
    {
        private readonly List<string> localizationKeys = new();
        private bool keysLoaded = false;
        private StringTable stringTable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!keysLoaded)
            {
                LoadLocalizationKeys();
                keysLoaded = true;
            }

            // Começar a propriedade
            EditorGUI.BeginProperty(position, label, property);

            // Calcular retângulos
            var dialogueKeyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var otherPropertiesRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height - EditorGUIUtility.singleLineHeight - 2);

            // Desenhar o campo de diálogo
            SerializedProperty keyProperty = property.FindPropertyRelative("dialogueKey");
            SerializedProperty textProperty = property.FindPropertyRelative("dialogueText");
            int selectedIndex = Mathf.Max(localizationKeys.IndexOf(keyProperty.stringValue), 0);

            selectedIndex = EditorGUI.Popup(
                dialogueKeyRect,
                "Dialogue Key",
                selectedIndex,
                localizationKeys.ToArray()
            );

            if (selectedIndex >= 0 && selectedIndex < localizationKeys.Count)
            {
                keyProperty.stringValue = localizationKeys[selectedIndex];
                string localizedText = GetLocalizedText(localizationKeys[selectedIndex]);
                textProperty.stringValue = localizedText;

                if (localizedText == "Invalid key!")
                {
                    GUI.backgroundColor = Color.red;
                }
                else
                {
                    GUI.backgroundColor = Color.white;
                }
            }

            // Desenhar outras propriedades
            EditorGUI.PropertyField(otherPropertiesRect, property, true);

            // Finalizar a propriedade
            EditorGUI.EndProperty();
        }

        private void LoadLocalizationKeys()
        {
            localizationKeys.Clear();
            var tableCollection = LocalizationEditorSettings.GetStringTableCollections().First(e => e.TableCollectionName is "GameLocalization");
            stringTable = tableCollection.StringTables[0];

            foreach (var entry in stringTable.Values)
            {
                localizationKeys.Add(entry.Key);
            }
        }

        private string GetLocalizedText(string key)
        {
            if (stringTable != null)
            {
                var entry = stringTable.GetEntry(key);

                if (entry != null)
                {
                    return entry.GetLocalizedString();
                }
            }

            return "Invalid key!";
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight + 2;
        }
    }
}