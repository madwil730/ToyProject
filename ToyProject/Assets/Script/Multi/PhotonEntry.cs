using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonEntry : MonoBehaviourPunCallbacks
{
	public PhotonView PV;
	public Text text;
	public Text ConnectText;
	private int num = 0;

	void Awake()
	{
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;

	}

	//서버 접속
	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings();
		ConnectText.text = "Connect";


	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.LocalPlayer.NickName = "A";
		PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
	}

	private void Update()
	{
		//text.text = num.ToString();
		//if(Input.GetKeyDown(KeyCode.A))
		//{
		//	PV.RPC("Plus", RpcTarget.AllBuffered);
		//}
	}

	[PunRPC]
	void Plus()
	{
		num++;
	}
}
