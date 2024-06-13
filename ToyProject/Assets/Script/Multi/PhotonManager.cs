using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{

	void Awake()
	{
		Screen.SetResolution(1600, 900,false);
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;

	}

	//���� ����
	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		if(PhotonNetwork.IsMasterClient)
			PhotonNetwork.LocalPlayer.NickName = "A";
		else
			PhotonNetwork.LocalPlayer.NickName = "B";

		PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
	}

	public override void OnJoinedRoom()
	{
		GameManager.Instance.player = PhotonNetwork.Instantiate("Character/Player", Vector3.zero, Quaternion.identity).GetComponent<Player>();

		GameManager.Instance.GameStart(0);

		//GameManager.Instance.FindRemotePlayerPhotonViews();
	}

	//public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	//{
	//	Debug.Log($"{newPlayer.NickName} joined the room.");

	//	// �÷��̾ ���� ��, �ش� �÷��̾��� ������Ʈ�� ã���ϴ�.
	//	foreach (GameObject go in  GameObject.FindGameObjectsWithTag("Player"))
	//	{
	//		Debug.Log(7878);
	//		PhotonView pv = go.GetComponent<PhotonView>();
	//		if (pv.Owner == newPlayer)
	//		{
	//			Debug.Log($"New Player's Object PhotonView ID: {pv.ViewID}");
	//		}
	//	}
	//}
}
