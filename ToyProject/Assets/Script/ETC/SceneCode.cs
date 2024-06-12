using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCode : MonoBehaviour
{
	public float speed = 5f; // 탄환 속도
	private Vector2 target;

	// 목표 설정
	public void SetTarget(Vector2 target)
	{
		this.target = target;
		Vector2 direction = (target - (Vector2)transform.position).normalized;
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}
}
