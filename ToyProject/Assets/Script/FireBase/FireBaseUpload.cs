using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class FireBaseUpload : MonoBehaviour
{
	float Version;
	public void AssetUpload()
	{
		StartCoroutine(upload());
	}

	private IEnumerator upload()
	{
		string modelPath = Path.Combine(Application.dataPath, "Manifast/weapondata");

		//model
		FirebaseStorage storage = FirebaseStorage.DefaultInstance;
		var storageReference = storage.GetReference("weapondata");
		var uploadTask = storageReference.PutFileAsync(modelPath, null,
		new StorageProgress<UploadState>(state =>
		{
			Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
		}), CancellationToken.None, null);

		yield return new WaitUntil(() => uploadTask.IsCompleted);

		if (uploadTask.Exception != null)
		{
			Debug.LogError($"Failed upload : {uploadTask.Exception}");
			yield break;
		}
		Debug.Log("##############Upload data###############");

		var newMetadata = new MetadataChange
		{
			CustomMetadata = new Dictionary<string, string> {
				{"Version", Version.ToString()}
			}
		};

		// Update metadata properties
		storageReference.UpdateMetadataAsync(newMetadata).ContinueWithOnMainThread(task => {
			if (!task.IsFaulted && !task.IsCanceled)
			{
				// access the updated meta data
				StorageMetadata meta = task.Result;
			}
		});

		Debug.Log("##############Upload metaData###############");
	}
}
