using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevlUp : MonoBehaviour
{
    private RectTransform rect;
	private Item[] items;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		items = GetComponentsInChildren<Item>(true);
	}
	
	public void Show()
	{
		Next();
		rect.localScale = Vector3.one;
		GameManager.Instance.Stop();
		AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
		AudioManager.instance.EfflectBgm(true);
	}

	public void Hide()
	{
		rect.localScale = Vector3.zero;
		GameManager.Instance.Resume();
		AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
		AudioManager.instance.EfflectBgm(false);
	}

	public void Select (int index)
	{
		items[index].OnClick();
	}

	void Next()
	{
		foreach (Item item in items) 
		{
			item.gameObject.SetActive(false);	
		}

		int[] ran = new int[3];
		while (true) 
		{
			ran[0] = Random.Range(0, items.Length);
			ran[1] = Random.Range(0, items.Length);
			ran[2] = Random.Range(0, items.Length);

			if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
				break;
		}

		for(int index = 0; index< ran.Length; index++)
		{
			Item ranItem = items[ran[index]];

			//만렙 아이템의 경우는 소비아이템으로 대체
			if (ranItem.level == ranItem.data.damage.Length)
				items[4].gameObject.SetActive(true);
			else
				ranItem.gameObject.SetActive(true);
		}
	}
}
