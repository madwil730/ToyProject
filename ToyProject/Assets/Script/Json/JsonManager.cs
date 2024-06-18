using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonManager : MonoBehaviour
{
	private string filePath;

	void Start()
	{
		filePath = Path.Combine(Application.streamingAssetsPath, "Version.json");

	
	}



	public float checkVersion()
	{
		if (File.Exists(filePath))
		{
			string jsonContent = File.ReadAllText(filePath);
			JsonData config = JsonConvert.DeserializeObject<JsonData>(jsonContent);
			return config.Version;
		}
		else
			return 0;
		
	}

	public void WriteJson(JsonData config)
	{
		string jsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
		File.WriteAllText(filePath, jsonContent);
	}
}

[System.Serializable]
public class JsonData
{
	public float Version;
}