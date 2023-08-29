using UnityEngine;
using UnityEditor;
using NetSpell.SpellChecker;

public class SpellCheckWindow : EditorWindow
{
    [MenuItem("Spelling/Open")]
    public static void Open()
    {
        CreateWindow<SpellCheckWindow>();
    }

    [TextArea(5, 25)]
    public string text;

    Spelling spellCheck;
    SerializedObject obj;

    private void OnEnable()
    {
        obj = new SerializedObject(this);
        spellCheck = new Spelling();
    }

    private void OnGUI()
    {
        EditorGUILayout.PropertyField(obj.FindProperty("text"));
        obj.ApplyModifiedProperties();


        if (GUILayout.Button("Check Spelling"))
        {
            spellCheck.SpellCheck(text);
            spellCheck.Suggest(text);
        }
    }
}
