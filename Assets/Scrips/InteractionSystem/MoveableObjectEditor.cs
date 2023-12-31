﻿#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveableObject))]
public class MoveableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MoveableObject moveableObject = (MoveableObject)target;

        if (GUILayout.Button("Assign Current Values At Index"))
        {
            moveableObject.AssignTranformValues();
        }

        if (GUILayout.Button("Set back to Initial position"))
        {
            moveableObject.SetToInitialPosition();
        }

        if (GUILayout.Button("Set to index position"))
        {
            moveableObject.SetToAssignIndexPosition();
        }
    }
}

#endif

