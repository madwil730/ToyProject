using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManage : MonoBehaviourPunCallbacks
{
    public InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject RespawnPanel;

	private void Awake()
	{
        //Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
	}

    //Æ÷Åæ Ä¿³ØÆ®
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

	public override void OnConnectedToMaster()
	{
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
	}

    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);   
    }

	private void Update()
	{
        // ¼­¹ö ²÷±â
        if (Input.GetKeyUp(KeyCode.Escape) && PhotonNetwork.IsConnected) 
            PhotonNetwork.Disconnect();
	}

    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }

}
