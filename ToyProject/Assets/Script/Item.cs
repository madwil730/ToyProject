using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
	private Gear gear;

	private Image icon;
	private Text textlevel;
	private Text textName;
	private Text textDesc;

	private void Awake()
	{
		icon  = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textlevel = texts[0];
		textName = texts[1];
		textDesc = texts[2];
		textName.text = data.itemName;
	}

	private void OnEnable()
	{
		textlevel.text = "Lv." + (level + 1);

		switch(data.itemType)
		{
			case ItemData.ItemType.Melee:
			case ItemData.ItemType.Range:
				textDesc.text = string.Format(data.itemDesc, data.damage[level] * 100, data.counts[level]);
				break;

			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoe:
				textDesc.text = string.Format(data.itemDesc, data.damage[level] * 100);
				break;
			default:
				textDesc.text = string.Format(data.itemDesc);
				break;
		}
	}

	public void OnClick()
	{
		switch(data.itemType)
		{
			case ItemData.ItemType.Melee:
			case ItemData.ItemType.Range:
				if(level == 0)
				{
					weapon.Init(data);
				}
				else
				{
					float nextDamage = data.baseDamage;
					int nextCount = 0;

					nextDamage += data.baseDamage * data.damage[level];
					nextCount += data.counts[level];

					weapon.LevelUp(nextDamage, nextCount);	
				}

				level++;
				break;

			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoe:
				if (level == 0)
				{
					GameObject newGear = new GameObject();
					gear = newGear.AddComponent<Gear>();
					gear.Init(data);
				}
				else
				{
					float nextRate = data.damage[level];
					gear.LevelUp(nextRate);
				}

				level++;
				break;
			case ItemData.ItemType.Heal:
				GameManager.Instance.health = GameManager.Instance.maxHealth;

				break;
		}


		if(level == data.damage.Length)
		{
			GetComponent<Button>().interactable = false;
		}
	}
}


