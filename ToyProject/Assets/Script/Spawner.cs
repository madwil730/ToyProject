using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

	private void Awake()
	{
		spawnPoint = GetComponentsInChildren<Transform>();
	

	}

	private void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

		timer += Time.deltaTime;
		level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length - 1);

		//if (timer > 1)
		//	return;
		//else
		//	if (PhotonNetwork.IsMasterClient)
		//	Spawn();
		//if (timer > spawnData[level].spawnTime)
		//{
		//    timer = 0;
		//    Spawn();

		//}

		if (Input.GetKeyDown(KeyCode.A) &&PhotonNetwork.IsMasterClient )
			Spawn();
	}



    public void Spawn()
    {
		GameObject enemy = GameManager.Instance.pool.Get("Character/", 0);
		enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
		PhotonView photonView = enemy.GetComponent<PhotonView>();
		photonView.RPC("Init", RpcTarget.AllBuffered, spawnData[level].spriteType, spawnData[level].speed, spawnData[level].health);
	}
}


[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
