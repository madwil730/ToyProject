using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    public List<GameObject>[] pools;

	private void Awake()
	{
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++) 
        {
            pools[index] = new List<GameObject>();
        }
	}

	public GameObject Get(string str, int prefabindex)
	{
		GameObject select = null;

		if (!select)
		{
			select = PhotonNetwork.Instantiate(str + prefabs[prefabindex].name,new Vector3(500,500,0), Quaternion.identity);
			pools[prefabindex].Add(select);
		}

		return select;
	}


	public GameObject  Spawn(string str, int prefabindex, Vector3 vec)
	{
		GameObject select = null;

		Debug.Log(vec);

		if (!select)
		{
			select = PhotonNetwork.Instantiate(str + prefabs[prefabindex].name, vec, Quaternion.identity);
			pools[prefabindex].Add(select);
		}

		return select;
	}

}
