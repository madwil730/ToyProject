using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCode : MonoBehaviour
{
	public float speed = 5f; // źȯ �ӵ�
	private Vector2 target;

	// ��ǥ ����
	public void SetTarget(Vector2 target)
	{
		this.target = target;
		Vector2 direction = (target - (Vector2)transform.position).normalized;
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}
}
