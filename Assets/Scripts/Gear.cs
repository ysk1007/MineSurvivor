using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public string itemName;
    public float rate;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localScale = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        itemName = data.itemName;
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
            case ItemData.ItemType.Skill:
                SkillUp(itemName);
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
/*                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;*/
                default:
                    float speed = 1 * Character.WeaponSpeed;
                    speed = 1f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }

    void SkillUp(string itemName)
    {
        switch (itemName)
        {
            case "광란":
                GameManager.instance.player.SkillCoolTime[0] -= rate;
                break;
            case "아드레날린":
                GameManager.instance.player.BSK_Level += (int)Math.Round(rate);
                break;
            case "파죽지세":
                GameManager.instance.player.SkillDuration[0] += rate;
                break;
        }
    }

    public void CompletionSkill(string itemName)
    {
        switch (itemName)
        {
            case "광란":
                GameManager.instance.player.SkillMaster1 = true;
                break;
            case "아드레날린":
                GameManager.instance.player.SkillMaster2 = true;
                break;
            case "파죽지세":
                GameManager.instance.player.SkillMaster3 = true;
                break;
        }
    }
}
