using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;
	[HideInInspector]
	public int prefabId;
	[HideInInspector]
	public float damage;
	//[HideInInspector]
	//public int count;
	[HideInInspector]
	public float speed;

	[HideInInspector]
	public int per;

	private Rigidbody2D rigid;

	public PhotonView pv;

	private float timer;
    private Player player;

	private void Awake()
	{
		player = GameManager.Instance.player;
		rigid = GetComponent<Rigidbody2D>();
	}


	public void Init(float damage, int per, Vector3 dir)
	{
		this.damage = damage;
		this.per = per;

		if (per > -1)
		{
			rigid.velocity = dir * 15f;
		}
	}

	public void Init(ItemData data)
    {
		player = GameManager.Instance.player;


        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
		GameManager.Instance.shovelCount = data.baseCount + Character.Count;


        for(int index  =0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if(data.prefab == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        { 
            case 0:
				GameManager.Instance.shovelSpeed = 80 * Character.WeaponSpeed;
                ReadyShovel();
                break;

            default:
                speed = 0.5f * Character.WeaponRate;
				break; 
        
        }
    }


	bool isbool; 
	public void LevelUp(float damage, int count)
	{
		this.damage = damage * Character.Damage;
		GameManager.Instance.shovelCount += count;
		//this.count += count;
		isbool = true;

		Debug.Log(151235);

		if (id == 0)
			ReadyShovel();

		//player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
	}

	private void ReadyShovel()
    {
		Debug.Log(GameManager.Instance.shovelCount + "  is count");
		Debug.Log(isbool);
		for (int index = 0; index < GameManager.Instance.shovelCount;  index++)
        {

			if (index < player.Center.childCount)
			{
				PhotonView photonView = player.Center.GetChild(index).GetComponent<PhotonView>();
				if (photonView != null)
				{
					photonView.RPC("SetParentRPC", RpcTarget.AllBuffered, GameManager.Instance.player.PV.ViewID, index);
				}
			}
			else
			{
				GameManager.Instance.pool.ShovelGet(prefabId, index);
				
			}
		}
    }


	void SetParentRPC2( int index)
	{
		Debug.Log(GameManager.Instance.shovelCount);
		Debug.Log(isbool);
		//PhotonView parentPhotonView = PhotonView.Find(parentViewID);
		{
			transform.SetParent(player.Center, true);


			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			Vector3 rotVec = Vector3.forward * 360 * index / GameManager.Instance.shovelCount;
			Debug.Log(rotVec);
			this.transform.Rotate(rotVec);
			this.transform.Translate(this.transform.up * 1.5f, Space.World);
			Init(damage, -1, Vector3.zero);
		}
	}



	[PunRPC]
	void SetParentRPC(int parentViewID, int index)
	{
		Debug.Log(GameManager.Instance.shovelCount);
		Debug.Log(isbool);
		PhotonView parentPhotonView = PhotonView.Find(parentViewID);
		if (parentPhotonView != null)
		{
			transform.SetParent(parentPhotonView.transform.Find("Center"), true);


			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			Vector3 rotVec = Vector3.forward * 360 * index / GameManager.Instance.shovelCount;
			Debug.Log(rotVec);
			this.transform.Rotate(rotVec);
			this.transform.Translate(this.transform.up * 1.5f, Space.World);
			Init(damage, -1, Vector3.zero);
		}
	}
	void ReadyFire()
	{
		//      if (player.scaneer.nearestTarget == null)
		//          return;

		//      Vector3 targetPos = player.scaneer.nearestTarget.position;
		//      Vector3 dir = targetPos - transform.position;
		//      dir = dir.normalized;

		//      Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
		//      bullet.position = transform.position;
		//      bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
		//      bullet.GetComponent<Bullet>().Init(damage, count, dir);

		//AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
	}


	void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

		switch (id)
		{
			case 0:

				break;

			default:
				timer += Time.deltaTime;

				if (timer > speed)
				{
					timer = 0;
					ReadyFire();
				}
				break;
		}

		//Debug.Log(GameManager.Instance.shovelCount);

		//if (Input.GetKeyDown(KeyCode.W))
		//	GameManager.Instance.shovelCount++;
		//player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
	}

	

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Enemy") || per == -1)
			return;

		per--;

		if (per == -1)
		{
			rigid.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
}
