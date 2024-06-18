using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
	[SerializeField]
	private string AssetBundle_Prefab;
	private AssetBundle prefabBundle;
	[HideInInspector]
	public GameObject[] prefab;


	public void AssetBundleLoad()
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), Application.streamingAssetsPath);
		string prefabPath = Path.Combine(path, AssetBundle_Prefab);

		StartCoroutine(LoadFromMemoryAsync(prefabPath));
	}

	private IEnumerator LoadFromMemoryAsync(string prefabPath)
	{
		//model
		UnityWebRequest prefabRequest = UnityWebRequestAssetBundle.GetAssetBundle(prefabPath);
		yield return prefabRequest.SendWebRequest();

		if (prefabBundle != null)
			prefabBundle.Unload(true);

		prefabBundle = DownloadHandlerAssetBundle.GetContent(prefabRequest);
		prefab = prefabBundle.LoadAllAssets<GameObject>();



	}
}
