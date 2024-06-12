using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCode : MonoBehaviourPunCallbacks
{
	//public PhotonView PV;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//Debug.Log(GameManager.Instance.shovelSpeed);
        this.transform.Rotate(Vector3.back * GameManager.Instance.shovelSpeed * Time.deltaTime);
    }


}
