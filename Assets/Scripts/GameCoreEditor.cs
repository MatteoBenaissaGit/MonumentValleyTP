using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameCore))]
public class GameCoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GameCore gameCore = (GameCore)target;
        
        if (GUILayout.Button("Setup Neighbours"))
        {
            gameCore.SetupNeighbours();
        }

        EditorUtility.SetDirty(gameCore);
        serializedObject.ApplyModifiedProperties();
    }
}