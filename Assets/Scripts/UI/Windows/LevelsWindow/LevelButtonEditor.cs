#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(LevelButton))]
public class LevelButtonEditor : ButtonEditor
{
    private SerializedProperty _locked;
    private SerializedProperty _unlocked;
    private SerializedProperty _passed;
    private SerializedProperty _comingSoon;
    private SerializedProperty _levelNumberText;
    private SerializedProperty _levelNumber;

    protected override void OnEnable()
    {
        base.OnEnable();
        _locked = serializedObject.FindProperty("_locked");
        _unlocked = serializedObject.FindProperty("_unlocked");
        _passed = serializedObject.FindProperty("_passed");
        _comingSoon = serializedObject.FindProperty("_comingSoon");
        _levelNumberText = serializedObject.FindProperty("_levelNumberText");
        _levelNumber = serializedObject.FindProperty("_levelNumber");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_locked);
        EditorGUILayout.PropertyField(_unlocked);
        EditorGUILayout.PropertyField(_passed);
        EditorGUILayout.PropertyField(_comingSoon);
        EditorGUILayout.PropertyField(_levelNumberText);
        EditorGUILayout.PropertyField(_levelNumber);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif