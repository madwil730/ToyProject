using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public PhotonView PV;
	public PhotonView RemotePV;
	public Vector2 inputVec;
    public float speed;
    // 수정 
    public Scanner scaneer;
    public RuntimeAnimatorController[] animCon;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
	public Transform Center;
    [SerializeField]
	private RectTransform healthBar;
	public Transform parentTransform; // 부모가 될 Transform
	public float health= 20f;

	void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();    
        scaneer  = GetComponent<Scanner>();


		if (PV.IsMine)
		{
			// 2D 카메라
			var CM = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
			CM.Follow = transform;
			CM.LookAt = transform;
		}

	}


	private void OnEnable()
	{
        speed *= Character.Speed;

		if(PhotonNetwork.IsMasterClient)
		{
			if (PV.IsMine)
				anim.runtimeAnimatorController = animCon[GameManager.Instance.playerId];
			else
				anim.runtimeAnimatorController = animCon[1];
		}
		else
		{
			if (PV.IsMine)
				anim.runtimeAnimatorController = animCon[1];
			else
				anim.runtimeAnimatorController = animCon[0];
		}
	
	}

	// 플레이어 이동
	private void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

		//healthBar.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);

		Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + nextVec);

		anim.SetFloat("Speed", inputVec.magnitude);

		if (Input.GetKeyDown(KeyCode.V))
			if (PV.IsMine)
				Debug.Log(PV.ViewID);
		if (Input.GetKeyDown(KeyCode.B))
			if (!PV.IsMine)
				Debug.Log(PV.ViewID);


	}

	[PunRPC]
	void Flip()
	{
		if (inputVec.x != 0 && PV.IsMine)
		{
			spriter.flipX = inputVec.x < 0;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
        if (!GameManager.Instance.isLive || collision.collider.CompareTag("Player"))
            return;

       // GameManager.Instance.health -= Time.deltaTime * 10;
        //health -= Time.deltaTime * 10;

        if(health < 0)
        {
            for(int index =2;  index < transform.childCount; index ++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

			if (PV.IsMine)
				GameManager.Instance.Player1Dead = true;
			else if (!PV.IsMine)
				GameManager.Instance.Player2Dead = true;

			anim.SetTrigger("Dead");
			rigid.bodyType = RigidbodyType2D.Static;
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

	//[PunRPC]
	//private void NotMne()
	//{
	//	if(!PV.IsMine)
	//	Debug.Log("Not Mine");
	//}

	//[PunRPC]
	//private void Mne()
	//{
	//	Debug.Log("Mine");
	//}
}
