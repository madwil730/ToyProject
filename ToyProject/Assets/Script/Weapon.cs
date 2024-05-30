using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

	private void Awake()
	{
        player = GameManager.Instance.player;
	}

    public void LevelUp(float damage, int count)
    {
		this.damage = damage *  Character.Damage;
		this.count += count;

        if (id == 0)
            Batch();

		player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
	}

    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int index  =0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // 캐릭터 id 정보 받는 곳 
        switch (id)
        { 
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;

            default:
                speed = 0.5f * Character.WeaponRate;
				break; 
        
        }
    }

    void Batch()
    {
        for(int index = 0; index < count;  index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
     

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec  = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }

    }

    void Fire()
    {
        if (player.scaneer.nearestTarget == null)
            return;

        Vector3 targetPos = player.scaneer.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

		AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
	}


	void Update()
	{
		if (!GameManager.Instance.isLive)
			return;

		switch (id)
		{
			case 0:
				transform.Rotate(Vector3.back * speed * Time.deltaTime);
				break;

			default:
				timer += Time.deltaTime;

				if (timer > speed)
				{
					timer = 0;
					Fire();
				}
				break;
		}

		player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
	}
}
