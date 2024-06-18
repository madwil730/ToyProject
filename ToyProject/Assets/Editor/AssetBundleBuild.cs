using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuild : MonoBehaviour
{
	[MenuItem("Bundles/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{

		BuildPipeline.BuildAssetBundles("Assets/UploadAssetBundle", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
		AssetDatabase.Refresh();

		Debug.Log("AssetBundle Complete!");
	}
}
