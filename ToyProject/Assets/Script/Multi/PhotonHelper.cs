using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonHelper : MonoBehaviour
{
	[PunRPC]
	void SetParentRPC(int parentViewID, int index)
	{
		PhotonView parentPhotonView = PhotonView.Find(parentViewID);
		if (parentPhotonView != null)
		{
			transform.SetParent(parentPhotonView.transform, true);

			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			//Vector3 rotVec = Vector3.forward * 360 * index / count;
			//this.transform.Rotate(rotVec);
			//this.transform.Translate(this.transform.up * 1.5f, Space.World);
		}
	}
}
