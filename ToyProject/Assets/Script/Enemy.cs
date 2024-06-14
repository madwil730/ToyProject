using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviourPunCallbacks
{
    private float speed;
	public float health;
	private float maxHealth;	
	public RuntimeAnimatorController[] animCon;
    //private Rigidbody2D target;

    public bool isLive = true;
	public Scanner scanner;

	[SerializeField]
    private Rigidbody2D rigid;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private SpriteRenderer spriter;

	Vector2 vec2;

	private void OnEnable()
	{
		//target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
		isLive = true;
		coll.enabled = true;
		rigid.simulated = true;
		spriter.sortingOrder = 2;
		anim.SetBool("Dead", false);
		health = maxHealth;
	}

	private void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

		if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
			return;

		if (scanner.nearestTarget == null)
			return;
		//Vector2 dirVec = (Vector2)scanner.nearestTarget.position - rigid.position;
		//Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
		//rigid.MovePosition(rigid.position + nextVec);
		//rigid.velocity = Vector2.zero;

		//spriter.flipX = scanner.nearestTarget.position.x < rigid.position.x;

		//if (Vector2.Distance((Vector2)scanner.nearestTarget.position, rigid.position) > 10)
		//{
		//	Vector3 playerDir = GameManager.Instance.player.inputVec;
		//	float dirX = playerDir.x < 0 ? -1 : 1;
		//	float dirY = playerDir.y < 0 ? -1 : 1;

		//	transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
		//}

	}

	[PunRPC]
	public void Init(int spriteType, float speed, int health)
	{
		anim.runtimeAnimatorController = animCon[spriteType];
		this.speed = speed;
		maxHealth = health;
		this.health = health;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Bullet") || !isLive)
			return;


		health -= collision.GetComponent<WeaponAbstract>().damage;
	
		PhotonView photonView = GetComponent<PhotonView>();

		StartCoroutine(KnockBack());


		if (health > 0)
		{
			anim.SetTrigger("Hit");
			AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
		}
		else
		{
			isLive = false;
			
			spriter.sortingOrder = 1;
			coll.enabled = false;
			rigid.simulated = false;
			anim.SetBool("Dead", true);


			if ( collision.tag == "Bullet" && collision.GetComponent<PhotonView>().IsMine)
			{
				GameManager.Instance.kill++;
				GameManager.Instance.GetExp();
			}
			

			if (GameManager.Instance.isLive)
				AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
		}
	}


	IEnumerator KnockBack()
	{
		yield return null;
		Vector3 playerPos = GameManager.Instance.player.transform.position;
		Vector3 dirVec = transform.position - playerPos;
		rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
	}

	public void Dead()
	{
		gameObject.SetActive(false);
	}
}

