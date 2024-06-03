using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PraticeCode : MonoBehaviourPunCallbacks
{

	[PunRPC]
	void SetParentRPC(int parentViewID)
	{
		PhotonView parentPhotonView = PhotonView.Find(parentViewID);
		if (parentPhotonView != null)
		{
			transform.SetParent(parentPhotonView.transform, true);
		}
	}

	
}
