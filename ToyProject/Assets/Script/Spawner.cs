using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

	private void Awake()
	{
		spawnPoint = GetComponentsInChildren<Transform>();   
	}

    // Update is called once per frame
    void Update()
    {
		if (!GameManager.Instance.isLive)
			return;

		timer += Time.deltaTime;
        level = Mathf.Min( Mathf.FloorToInt(GameManager.Instance.gameTime / 10f),spawnData.Length -1 );

        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
       GameObject enemy =    GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
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