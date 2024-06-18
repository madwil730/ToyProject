using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FireBaseUpload))]

public class FireBaseEditor : Editor
{
	private FireBaseUpload firebaseController;
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		firebaseController = (FireBaseUpload)target;

		if (GUILayout.Button("AssetUpload"))
		{
			firebaseController.AssetUpload();
			AssetDatabase.Refresh();
		}

		serializedObject.ApplyModifiedProperties();
	}
}

