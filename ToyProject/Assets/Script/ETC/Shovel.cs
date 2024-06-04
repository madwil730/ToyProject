using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviourPunCallbacks
{

	[HideInInspector]
	public int id;
	[HideInInspector]
	public int prefabId;
	[HideInInspector]
	public float damage;
	[HideInInspector]
	public float speed;
	[HideInInspector]
	public int per;

	private Rigidbody2D rigid;
	public PhotonView pv;

	private float timer;
	private Player player;


	public void Init(ItemData data)
	{
		player = GameManager.Instance.player;


		// Property Set
		id = data.itemId;
		damage = data.baseDamage * Character.Damage;
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
				break;

			default:
				speed = 0.5f * Character.WeaponRate;
				break;

		}
	}

	public void LevelUp(float damage, int count)
	{
		this.damage = damage * Character.Damage;
		GameManager.Instance.shovelCount += count;
	}

	[PunRPC]
	void SetParentRPC(int parentViewID, int index, int Count)
	{
		PhotonView parentPhotonView = PhotonView.Find(parentViewID);
		if (parentPhotonView != null)
		{
			transform.SetParent(parentPhotonView.transform.Find("Center"), true);


			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			Vector3 rotVec = Vector3.forward * 360 * index / Count;
			Debug.Log(GameManager.Instance.shovelCount);
			Debug.Log(rotVec);
			this.transform.Rotate(rotVec);
			this.transform.Translate(this.transform.up * 1.5f, Space.World);
			//Init(damage, -1, Vector3.zero);
		}
	}
}
