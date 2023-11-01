using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Pickax, Dynamite, Range, Glove, Shoe, Heal }

    [Header(" # Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header(" # Level Info")]
    public float baseDamge;
    public int baseCount;
    public float[] damages;
    public int[] count;

    [Header(" # Weapon")]
    public GameObject projectile;

}
