using UnityEditor;
using UnityEngine;

public class MovementEditorWindow : EditorWindow
{
    private PlayerMovementData movementData;

    [MenuItem("Window/Movement")]
    public static void ShowWindow()
    {
        GetWindow<MovementEditorWindow>("Movement Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Movement Editor", EditorStyles.boldLabel);

        movementData = (PlayerMovementData)EditorGUILayout.ObjectField(
            "Movement Data", movementData, typeof(PlayerMovementData), false
        );

        if (movementData != null)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            movementData.moveSpeed = EditorGUILayout.FloatField("Move Speed", movementData.moveSpeed);
            movementData.jumpForce = EditorGUILayout.FloatField("Jump Force", movementData.jumpForce);
            movementData.gravityScale = EditorGUILayout.FloatField("Gravity Scale", movementData.gravityScale);

            EditorGUILayout.Space();
            if (GUILayout.Button("Save Changes"))
            {
                EditorUtility.SetDirty(movementData);
                AssetDatabase.SaveAssets();
                Debug.Log("Movement data saved!");
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a PlayerMovementData asset to edit.", MessageType.Info);
        }
    }
}
