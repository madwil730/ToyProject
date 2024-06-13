using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponAbstract
{
	private float waitTimer;
	public float weaponSpeed = 0.1f;
	public int PenetrationCount = 0;
	public int Penetration = 0;
	private void Awake()
	{
		player = GameManager.Instance.player;
		rigid = GetComponent<Rigidbody2D>();
	}

	[PunRPC]
	void ReadyFire()
	{
		Debug.Log(523532);
		//if (GameManager.Instance.player2P == null)
		//	GameManager.Instance.FindRemotePlayerPhotonViews();

		//if (GameManager.Instance.player2P != null)
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
				this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // 총알을 목표 방향으로 회전
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
				this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // 총알을 목표 방향으로 회전
				rigid.velocity = direction * 10f;
			}

		}

	}


	void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

	
				waitTimer += Time.deltaTime;
			
				if (waitTimer > weaponSpeed)
				{
					Debug.Log(weaponSpeed);
					waitTimer = 0;
					ReadyFire();
				}

		if (Input.GetKeyDown(KeyCode.Space) && id == 1)
			pv.RPC("ReadyFire", RpcTarget.AllBuffered);
	}

	// 쏠때마다 총알 관통 빼는거 재수정 
	public void SetPenetration()
	{
		Penetration = PenetrationCount;
	}

	// 총알 관통력 설정
	[PunRPC]
	public void SetPenetrationCount(int count)
	{
		PenetrationCount = count;
	}


	//탄알 관통력
	void OnTriggerEnter2D(Collider2D collision)
	{

		if (!collision.CompareTag("Enemy") || Penetration == -1 && id == 1)
			return;

		Penetration--;

		if (Penetration == 0)
		{
			rigid.velocity = Vector2.zero;
			this.transform.position = new Vector3(500, 500, 0);
		}
	}

	[PunRPC]
	public override void Init(float damage)
	{
		this.damage = damage;	
	}
}
