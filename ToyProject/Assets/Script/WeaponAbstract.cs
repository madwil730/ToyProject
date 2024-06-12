using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAbstract : MonoBehaviourPunCallbacks
{
	public int id;
	public float damage;
	public Rigidbody2D rigid;
	public PhotonView pv;
	public Player player;


}
