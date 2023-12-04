using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(PressedButton), true)]
[CanEditMultipleObjects]
public class PressButtonEditor : ButtonEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("whilePressed"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("pressInterval"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("pressButtonDelay"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("pressButtonDelay"), true);
        this.serializedObject.ApplyModifiedProperties();

    }
}
