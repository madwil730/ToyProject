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

		//foreach(GameObject item in pools[index])
		//{
		//    if(!item.activeSelf)
		//    {
		//        select = item;
		//        select.SetActive(true);
		//        break;
		//    }
		//}

		if (!select)
		{
			select = PhotonNetwork.Instantiate(str + prefabs[prefabindex].name,Vector3.one, Quaternion.identity);
			pools[prefabindex].Add(select);
		}

		return select;
	}

}
