using UnityEngine;
using UnityEditor;
using NetSpell.SpellChecker;

[CustomPropertyDrawer(typeof(SpellCheckerAreaAttribute))]
public class SpellCheckerAreaAttributeDrawer : PropertyDrawer
{
    Spelling spellCheck;
    bool newSuggestions;
    Vector2 scroll;
    bool showSuggestions;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (spellCheck == null)
        {
            spellCheck = new Spelling();
            spellCheck.ShowDialog = false;
        }

        // Make a 5 line high area
        position.height = EditorGUIUtility.singleLineHeight * 5;
        EditorGUI.BeginChangeCheck();
        property.stringValue = EditorGUI.TextArea(position, property.stringValue, EditorStyles.textArea);

        position.y += position.height;
        position.height = EditorGUIUtility.singleLineHeight;

        // Do The Spellcheck Stuff
        if (newSuggestions)
        {
            spellCheck.SpellCheck(property.stringValue);
            spellCheck.Suggest(spellCheck.CurrentWord);
            newSuggestions = false;
        }

        if (spellCheck.CurrentWord != null)
        {
            Rect buttonArea = new Rect(position);
            buttonArea.width -= 100;
            EditorGUI.LabelField(buttonArea, string.Format("Misspelled Word {0}", spellCheck.CurrentWord));
            buttonArea.x += buttonArea.width + 5;
            buttonArea.width = 95;
            showSuggestions = GUI.Toggle(buttonArea, showSuggestions, new GUIContent("Suggestions"), GUI.skin.button);
        }

        if (showSuggestions)
        {
            Rect suggestionArea = new Rect(position);
            suggestionArea.width -= 10;
            suggestionArea.height = EditorGUIUtility.singleLineHeight * 5;
            suggestionArea.y += EditorGUIUtility.singleLineHeight;

            float viewPortHeight = EditorGUIUtility.singleLineHeight + (EditorGUIUtility.singleLineHeight * spellCheck.MaxSuggestions);
            Rect viewport = new Rect(new Vector2(), new Vector2(position.width - 25, viewPortHeight));

            Rect buttonRect = new Rect(new Vector2(), new Vector2((viewport.width / 2), EditorGUIUtility.singleLineHeight));

            scroll = GUI.BeginScrollView(suggestionArea, scroll, viewport);

            for (int i = 0; i < spellCheck.Suggestions.Count; i++)
            {
                string suggestion = spellCheck.Suggestions[i].ToString();

                buttonRect.x = buttonRect.width * (i % 2);
                buttonRect.y = EditorGUIUtility.singleLineHeight * Mathf.FloorToInt(i / 2);
                GUI.Button(buttonRect, new GUIContent(suggestion));

                if (GUI.Button(buttonRect, new GUIContent(suggestion)))
                {
                    UpdateText(property, spellCheck.CurrentWord, suggestion);
                }
            }

            // Draw Buttons
            GUI.EndScrollView();
        }

        void UpdateText(SerializedProperty prop, string oldWord, string newWord)
        {
            prop.stringValue = prop.stringValue.Replace(oldWord, newWord);
            GUIUtility.keyboardControl = 0;
            GUIUtility.hotControl = 0;
            prop.serializedObject.ApplyModifiedProperties();
            newSuggestions = true;
        }

    }
}
