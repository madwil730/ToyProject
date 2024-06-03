using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed;
	private float health;
	private float maxHealth;	
	public RuntimeAnimatorController[] animCon;
    private Rigidbody2D target;

    public bool isLive = true;

	[SerializeField]
    private Rigidbody2D rigid;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private SpriteRenderer spriter;

	private void OnEnable()
	{
		target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
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

		//Vector2 dirVec = target.position - rigid.position;
		//Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
		//rigid.MovePosition(rigid.position + nextVec);
		//rigid.velocity = Vector2.zero;

		//spriter.flipX = target.position.x < rigid.position.x;

		//if(Vector2.Distance(target.position,rigid.position) >10)
		//{
		//	Vector3 playerDir = GameManager.Instance.player.inputVec;
		//	float dirX = playerDir.x < 0 ? -1 : 1;
		//	float dirY = playerDir.y < 0 ? -1 : 1;

		//	transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
		//}

	
	}


	public void Init(SpawnData data)
	{
		anim.runtimeAnimatorController = animCon[data.spriteType];
		speed = data.speed;
		maxHealth = data.health;
		health = data.health;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Bullet") || !isLive)
			return;

		health -= collision.GetComponent<Weapon>().damage;
		StartCoroutine(KnockBack());

		if(health > 0)
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
			GameManager.Instance.kill++;
			GameManager.Instance.GetExp();

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

