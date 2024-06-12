using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : MonoBehaviourPunCallbacks
{

	[HideInInspector]
	public int id;
	[HideInInspector]
	public int prefabId;
	[HideInInspector]
	public float speed;
	private Weapon InitWeapon;
	private List<GameObject> shovelList;
	private List<GameObject> bulletList;


	public void Init(ItemData data)
	{
		// Property Set
		id = data.itemId;
		GameManager.Instance.shovelCount = data.baseCount + Character.Count;


		for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
		{
			if (data.prefab == GameManager.Instance.pool.prefabs[index])
			{
				prefabId = index;
				break;
			}
		}

		switch (id)
		{
			case 0:
				GameManager.Instance.shovelSpeed = 80 * Character.WeaponSpeed;
				GameManager.Instance.pool.Get("Weapon/", prefabId);
				GameManager.Instance.pool.Get("Weapon/", prefabId);
				GameManager.Instance.pool.Get("Weapon/", prefabId);

				shovelList = GameManager.Instance.pool.pools[prefabId];

				for (int i = 0; i < shovelList.Count; i++)
				{
					InitWeapon = shovelList[i].GetComponent<Weapon>();
					InitWeapon.damage = data.baseDamage * Character.Damage;
					InitWeapon.pv.RPC("SetParentRPC", RpcTarget.AllBuffered, GameManager.Instance.player.PV.ViewID, i, shovelList.Count);
				}
				break;

			case 1:
				GameObject bullet = GameManager.Instance.pool.Get("Weapon/", prefabId);
				bullet.transform.position = new Vector3(500, 500, 0);

				InitWeapon = bullet.GetComponent<Weapon>();
				InitWeapon.damage = data.baseDamage * Character.Damage;
				InitWeapon.weaponSpeed = 0.5f;
				InitWeapon.PenetrationCount = data.baseCount;
				//bullet.GetComponent<PhotonView>().RPC("ReadyFire", RpcTarget.AllBuffered);
			
				bulletList = GameManager.Instance.pool.pools[prefabId];

				break;

			default:
				speed = 0.5f * Character.WeaponRate;
				break;

		}
	}


	public void LevelUp(float damage, int count, ItemData data)
	{

		// Property Set
		id = data.itemId;

		for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
		{
			if (data.prefab == GameManager.Instance.pool.prefabs[index])
			{
				prefabId = index;
				break;
			}
		}

		if (id == 0)
		{
			GameManager.Instance.pool.Get("Weapon/", prefabId);

			for (int index = 0; index < shovelList.Count; index++)
			{
				shovelList[index].GetComponent<Weapon>().damage = damage * Character.Damage;
				shovelList[index].GetComponent<Weapon>().pv.RPC("SetParentRPC", RpcTarget.AllBuffered, GameManager.Instance.player.PV.ViewID, index, shovelList.Count);
			}
		}

		else if(id ==1 )
		{
			for (int index = 0; index < bulletList.Count; index++)
			{
				bulletList[index].GetComponent<Weapon>().PenetrationCount = count;
			}
		}
	}
}
