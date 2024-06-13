using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : WeaponAbstract
{
	[PunRPC]
	public override void Init(float damage)
	{
		this.damage = damage;
	}

	private void Awake()
	{
		player = GameManager.Instance.player;
	}

	[PunRPC]
	void SetParentRPC(int parentViewID, int index, int count)
	{
		PhotonView parentPhotonView = PhotonView.Find(parentViewID);
		if (parentPhotonView != null)
		{
			transform.SetParent(parentPhotonView.transform.Find("Center"), true);


			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			Vector3 rotVec = Vector3.forward * 360 * index / count;
			this.transform.Rotate(rotVec);
			this.transform.Translate(this.transform.up * 1.5f, Space.World);
		}
	}
}
