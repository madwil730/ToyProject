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
		Weapon[] weapons  = transform.parent.GetComponentsInChildren<Weapon>();	

		foreach (Weapon weapon in weapons) 
		{
			switch(weapon.id)
			{
				case 0:
					float speed = 150 * Character.WeaponRate;
					weapon.speed = 150 + (150 * rate);
					break;
				default:
					speed = 0.5f * Character.WeaponRate;
					weapon.speed = 0.5f *  (1f - rate);
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
