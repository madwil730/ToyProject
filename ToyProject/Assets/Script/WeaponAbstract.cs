using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAbstract : MonoBehaviourPunCallbacks
{
	public int id;
	[HideInInspector]
	public float damage;
	[HideInInspector]
	public Rigidbody2D rigid;
	[HideInInspector]
	public PhotonView pv;
	[HideInInspector]
	public Player player;

	public abstract void Init(float damage);


}
