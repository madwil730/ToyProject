using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
	public ItemData.ItemType type;
	private float rate;

	public void Init(ItemData data)
	{
		name = "Gear " + data.itemId;
		transform.parent = GameManager.Instance.player.transform;	
		transform.localPosition = Vector3.zero;

		type = data.itemType;
		rate = data.damage[0];
		ApplyGear();
	}

	public void LevelUp(float rate)
	{
		this.rate = rate;	
		ApplyGear();
	}

	public void ApplyGear()
	{
		switch (type) 
		{
			case ItemData.ItemType.Glove:
				RateUp();
				break;
			case ItemData.ItemType.Shoe:
				SpeedUp();
				break;
		}
	}

	void RateUp()
	{
		//WeaponAbstract[] weapons  = transform.parent.GetComponentsInChildren<WeaponAbstract>();	
		WeaponAbstract[] weapons = GameObject.FindObjectsOfType<WeaponAbstract>();

		foreach (WeaponAbstract weapon in weapons) 
		{
			Debug.Log(weapon.id);	
			switch(weapon.id)
			{
				//»ð
				case 0:
					break;
				// ÃÑ¾Ë
				case 1:
					weapon.GetComponent<Bullet>().weaponSpeed = rate;
					break;
			}
		}
	}

	void SpeedUp()
	{
		float speed = 3;
		GameManager.Instance.player.speed = speed + speed * rate;
	}
}
