using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PraticeCode : MonoBehaviourPunCallbacks
{
	public Camera camera;
	public GameObject bulletPrefab; // 탄환 프리팹
	public Transform firePoint; // 탄환 발사 위치

	void Update()
	{
		// 예: 마우스 클릭 시 발사
		if (Input.GetMouseButtonDown(0))
		{
			Fire();
		}
	}

	void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		SceneCode bulletScript = bullet.GetComponent<SceneCode>();

		// 목표 설정 (예: 마우스 위치)
		Vector2 target = camera.ScreenToWorldPoint(Input.mousePosition);
		bulletScript.SetTarget(target);
	}




}



















