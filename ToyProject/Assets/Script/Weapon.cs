using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;
	public float damage;
	[HideInInspector]
	public float weaponSpeed;
	[HideInInspector]
	public int PenetrationCount = 0;
	public int Penetration = 0;

	public Rigidbody2D rigid;
	public PhotonView pv;
	private float timer;
    private Player player;


	private void Awake()
	{
		player = GameManager.Instance.player;
		rigid = GetComponent<Rigidbody2D>();
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

	[PunRPC]
	void ReadyFire()
	{
	

		Debug.Log("is no rpc");
		//pv.Ownerwd

		if (GameManager.Instance.player2P == null)
			GameManager.Instance.FindRemotePlayerPhotonViews();

		if (GameManager.Instance.player2P != null)
		{ 
			SetPenetration();
			if (pv.IsMine)
			{
				if (player.scaneer.nearestTarget == null)
					return;

				Vector3 targetPos = player.scaneer.nearestTarget.position;
				Vector3 dir = targetPos - transform.position;
				dir = dir.normalized;


				this.transform.position = player.scaneer.transform.position;
				this.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);


				Vector2 direction = ((Vector2)targetPos - (Vector2)transform.position).normalized;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // �Ѿ��� ��ǥ �������� ȸ��
				rigid.velocity = direction * 10f;
			}
			else
			{
				if (GameManager.Instance.player2P.GetComponent<Player>().scaneer.nearestTarget == null)
					return;

				Vector3 targetPos = GameManager.Instance.player2P.GetComponent<Player>().scaneer.nearestTarget.position;
				Vector3 dir = targetPos - transform.position;
				dir = dir.normalized;


				this.transform.position = GameManager.Instance.player2P.GetComponent<Player>().scaneer.transform.position;
				this.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);


				Vector2 direction = ((Vector2)targetPos - (Vector2)transform.position).normalized;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // �Ѿ��� ��ǥ �������� ȸ��
				rigid.velocity = direction * 10f;
			}
		
		}

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

			case 1:
				timer += Time.deltaTime;
				if (timer > weaponSpeed)
				{
					timer = 0;
					//ReadyFire();
				}
				break;
		}

		if (Input.GetKeyDown(KeyCode.Space) && id == 1)
			pv.RPC("ReadyFire", RpcTarget.AllBuffered);
		//ReadyFire();

		if (Input.GetKeyDown(KeyCode.T) && id == 1)
			pv.RPC("test", RpcTarget.AllBuffered);

	}

	// �򶧸��� �Ѿ� ���� ���°� ����� 
	public void SetPenetration()
	{
		Penetration = PenetrationCount;
	}


	// �Ѿ� ����� ����
	[PunRPC]
	public void SetPenetrationCount(int count)
	{
		PenetrationCount = count;
	}




	//ź�� �����
	void OnTriggerEnter2D(Collider2D collision)
	{
		
		if (!collision.CompareTag("Enemy") || Penetration == -1 && id ==1)
			return;

		Debug.Log(2222);
		Debug.Log(Penetration);
		Penetration--;

		if (Penetration == 0)
		{
			rigid.velocity = Vector2.zero;
			this.transform.position = new Vector3(500, 500, 0);
		}

		//if (!pv.IsMine)
		//pv.RPC("test", RpcTarget.AllBuffered);
		
	}

	[PunRPC]
	private void test()
	{
		Penetration--;

		if (Penetration == 0)
		{
			rigid.velocity = Vector2.zero;
			this.transform.position = new Vector3(500, 500, 0);
		}
	}


}
