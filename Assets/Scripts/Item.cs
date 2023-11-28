using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;
    public Sprite point;
    public Image[] Levels;

    Image icon;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[2];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textName = texts[0];
        textDesc = texts[1];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        for (int i = 0; i < level + 1; i++)
        {
            Levels[i].sprite = point;
        }
        switch (data.itemType)
        {
            case ItemData.ItemType.Pickax:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.count[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Pickax:
            case ItemData.ItemType.Range:
                    float nextDamage = data.baseDamge;
                    int nextCount = 0;

                    nextDamage += data.baseDamge * data.damages[level];
                    nextCount += data.count[level];

                    GameManager.instance.weapon.GetComponent<Weapon>().LevelUp(nextDamage, nextCount);
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
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                break;
            case ItemData.ItemType.Heal:
                break;
        }

        level++;

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
