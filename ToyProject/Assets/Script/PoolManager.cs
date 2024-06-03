using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

	private void Awake()
	{
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++) 
        {
            pools[index] = new List<GameObject>();
        }
	}


    public GameObject ShovelGet(int prefabindex, int count)
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

        if(!select)
        {
            select = PhotonNetwork.Instantiate("Weapon/"+prefabs[prefabindex].name, Vector3.zero, Quaternion.identity);

            //부모 설정을 동기화하는 RPC 호출

            if (select != null)
            {
                PhotonView photonView = select.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    photonView.RPC("SetParentRPC", RpcTarget.AllBuffered, GameManager.Instance.player.PV.ViewID, count);
                }
            }

            pools[prefabindex].Add(select);       
        }

        return select;  
    }

	public GameObject Get(int prefabindex)
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
			select = PhotonNetwork.Instantiate("Character/" + prefabs[prefabindex].name,Vector3.one, Quaternion.identity);
			pools[prefabindex].Add(select);
		}

		return select;
	}

}
