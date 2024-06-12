using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PraticeCode : MonoBehaviourPunCallbacks
{
	public Camera camera;
	public GameObject bulletPrefab; // źȯ ������
	public Transform firePoint; // źȯ �߻� ��ġ

	void Update()
	{
		// ��: ���콺 Ŭ�� �� �߻�
		if (Input.GetMouseButtonDown(0))
		{
			Fire();
		}
	}

	void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		SceneCode bulletScript = bullet.GetComponent<SceneCode>();

		// ��ǥ ���� (��: ���콺 ��ġ)
		Vector2 target = camera.ScreenToWorldPoint(Input.mousePosition);
		bulletScript.SetTarget(target);
	}




}



















