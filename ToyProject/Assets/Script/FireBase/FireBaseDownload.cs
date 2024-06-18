using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class FireBaseDownload : MonoBehaviour
{
	[SerializeField]
	private AssetBundleLoader assetBundleLoader;
	[SerializeField]
	private JsonManager jsonManager;

	float updateVersion;


	public void AssetDownLoad()
	{
		StartCoroutine(download());
	}

	private IEnumerator download()
	{


		var path = Path.Combine(Directory.GetCurrentDirectory(), Application.streamingAssetsPath);


		FirebaseStorage storage = FirebaseStorage.DefaultInstance;
		var storageReference = storage.GetReference("weaponData");


		// Get metadata properties
		var downloadMetaTask = storageReference.GetMetadataAsync().ContinueWithOnMainThread(task => {
			if (!task.IsFaulted && !task.IsCanceled)
			{
				StorageMetadata meta = task.Result;
				//meta.GetCustomMetadata("Version");
				float.TryParse(meta.GetCustomMetadata("Version"), out updateVersion);
				// do stuff with meta
			}
		});

		yield return new WaitUntil(() => downloadMetaTask.IsCompleted);

		if(jsonManager.checkVersion() < updateVersion)
		{
			var downloadTask = storageReference.GetFileAsync(path + "/weaponData",
			new StorageProgress<DownloadState>(state =>
			{
				Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
			}), CancellationToken.None);

			yield return new WaitUntil(() => downloadTask.IsCompleted);

			
			assetBundleLoader.AssetBundleLoad();
			JsonData config = new JsonData();
			config.Version = updateVersion;
			// 업데이트한 버전 json에 저장 
			jsonManager.WriteJson(config);	

			Debug.Log("##########manifast downloaded#############");
		}
		else
			Debug.Log("##########업데이트 파일 없음 #############");

	}
}
