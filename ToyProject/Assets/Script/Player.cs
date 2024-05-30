using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    // 수정 
    public Scanner scaneer;
    public RuntimeAnimatorController[] animCon;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    [SerializeField]
	private RectTransform healthBar;

	void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();    
        scaneer  = GetComponent<Scanner>();  
    }

	private void OnEnable()
	{
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.Instance.playerId];
	}

    // 플레이어 이동
	private void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

		healthBar.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);

		Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

		anim.SetFloat("Speed", inputVec.magnitude);

		if (inputVec.x != 0)
		{
			spriter.flipX = inputVec.x < 0;
		}
	}


	private void OnCollisionStay2D(Collision2D collision)
	{
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if(GameManager.Instance.health < 0)
        {
            for(int index =2;  index < transform.childCount; index ++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
	}

    // 조이스틱 input 시스템
	private void OnMove(InputValue value)
	{
        if (!GameManager.Instance.isLive)
            return;
        inputVec = value.Get<Vector2>();
	}
}
