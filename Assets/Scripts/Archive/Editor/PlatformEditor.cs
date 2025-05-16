#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Platform))]
public class PlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var plat = (Platform)target;
        var asm = Assembly.Load("Assembly-CSharp");  // for packages/other asm, adjust as needed
        var types = asm
            .GetTypes()
            .Where(t => t.IsEnum)
            .ToArray();
        int selType = Mathf.Max(0, System.Array.FindIndex(types, t => t.FullName == plat.enumTypeName));
        selType = EditorGUILayout.Popup("Enum Type", selType,
                        System.Array.ConvertAll(types, t => t.Name));
        plat.enumTypeName = types[selType].FullName;

        var eType = types[selType];
        var names = System.Enum.GetNames(eType);
        plat.enumValueIndex = EditorGUILayout.Popup("Enum Value",
                                 plat.enumValueIndex, names);

        DrawDefaultInspector();
        if (GUI.changed)
            EditorUtility.SetDirty(plat);
    }
}
#endif