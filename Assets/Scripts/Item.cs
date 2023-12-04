using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        textName.text = data.itemName + " " + string.Concat(Enumerable.Repeat("I", level+1));
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
            case ItemData.ItemType.Skill:
                if (level == 2)
                {
                    if (data.itemName == "아드레날린")
                    {
                        Player p = GameManager.instance.player;
                        textDesc.text = string.Format(data.itemDesc, p.BSK_Damage[p.BSK_Level+1] * 100, p.BSK_Speed[p.BSK_Level+1] * 100);
                        textDesc.text += data.SpecialDesc;
                    }
                    else
                    {
                        textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                        textDesc.text += data.SpecialDesc;
                    }
                }
                else
                {
                    if (data.itemName == "아드레날린")
                    {
                        Player p = GameManager.instance.player;
                        textDesc.text = string.Format(data.itemDesc, p.BSK_Damage[p.BSK_Level + 1] * 100, 100 - p.BSK_Speed[p.BSK_Level + 1] * 100);
                    }
                    else
                        textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                }
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
            case ItemData.ItemType.Skill:
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
                    if (level == 2)
                    {
                        gear.CompletionSkill(data.itemName);
                    }
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
