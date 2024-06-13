using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[Header("# Game Control")]
	public bool isLive;
	public bool Player1Dead;
	public bool Player2Dead;
	public float gameTime;
	public float maxGameTime = 2 * 10f;
	[Header("# Player Info")]
	public int playerId;
	public float health;
	public float maxHealth;
	public int level;
	public int kill;
	public int exp;
	public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
	[Header("# Game Object")]
	public Player player;
	public GameObject player2P;
	public PoolManager pool;
	public WeaponManager weaponManager;
	public LevlUp uiLevelUp;
	public Result uiResult;
	public Transform uiJoy;
	public GameObject enemyCleaner;
	
	//WeaponDatas
	[HideInInspector]
	public float shovelSpeed;
	public int shovelCount = 3;

	private void Awake()
	{
		Instance = this;

	}

	public void GameStart(int id)
	{
		playerId = id;
		health = maxHealth;
		uiLevelUp.Select(playerId );
		Resume();

		AudioManager.instance.PlayBgm(true);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
	}

	public void GameOver()
	{
		if(Player1Dead && Player2Dead)
		StartCoroutine(GameOverRoutine());
	}

	IEnumerator GameOverRoutine()
	{
		isLive = false;
		yield return new WaitForSeconds(0.5f);
		uiResult.gameObject.SetActive(true);
		uiResult.Lose();
		Stop();

		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
	}

	public void GameVictroy()
	{
		StartCoroutine(GameVictoryRoutine());
	}

	IEnumerator GameVictoryRoutine()
	{
		isLive = false;
		enemyCleaner.SetActive(true);

		yield return new WaitForSeconds(0.5f);

		uiResult.gameObject.SetActive(true);
		uiResult.Win();
		Stop();
		AudioManager.instance.PlayBgm(false);
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
	}

	public void GameRetry()
	{
		SceneManager.LoadScene(0);
	}

	public void GameQuit()
	{
		Application.Quit();
	}



	private void Update()
	{
		if (!isLive)
			return;
		gameTime += Time.deltaTime;

		if(gameTime > maxGameTime)
		{
			gameTime = maxGameTime;
			GameVictroy();
		}

		if (Input.GetKeyDown(KeyCode.R))
			uiLevelUp.Show();

		if (player2P == null)
			FindRemotePlayerPhotonViews();
	}

	public void GetExp()
	{
		if (!isLive)
			return;

		exp++;
		if(exp  == nextExp[Mathf.Min(level,nextExp.Length-1)])
		{
			level++;
			exp = 0;
			uiLevelUp.Show();	
		}
	}

	public void Stop()
	{
		isLive = false;	
		Time.timeScale = 0;
		uiJoy.localScale = Vector3.zero;
	}

	public void Resume()
	{
		isLive= true;
		Time.timeScale = 1;
		uiJoy.localScale = Vector3.one;
	}


	public void FindRemotePlayerPhotonViews()
	{
		Photon.Realtime.Player[] allPlayers = PhotonNetwork.PlayerList;
		//Debug.Log(allPlayers.Length);

		foreach (Photon.Realtime.Player player in allPlayers)
		{
			if (!player.IsLocal) // 로컬 플레이어가 아닌 경우
			{
				//Debug.Log("Found a remote player: " + player.NickName + ", ActorNumber: " + player.ActorNumber);

				// 해당 리모트 플레이어의 PhotonView 찾기
				PhotonView[] photonViews = FindObjectsOfType<PhotonView>();


				foreach (PhotonView pv in photonViews)
				{
					if ( pv.Owner == player && pv.gameObject.GetComponent<Player>() != null)
					{
						player2P = pv.gameObject;
						//Debug.Log("PhotonView ID: " + pv.ViewID + " belongs to remote player: " + player.NickName);
					}
				}
			}
		}
	}


}



