using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "Scriptable Object/DungeonData")]
public class DungeonData : ScriptableObject
{
    public enum Difficulty { EASY, NORMAL, HARD, HELL }

    [Header(" # Main Info")]
    public Difficulty difficulty;
    public int DungeonID;
    public string DungeonName;
    [TextArea]
    public string DungeonDesc;

    [Header(" # Sprite Info")]
    public Sprite MapBgSprite;
    public Sprite BossSprite;

    [Header(" # Enemy Info")]
    public Sprite[] Enemyicons;
    public Sprite Bossicon;

    [Header(" # Object")]
    public GameObject Frame;
    public GameObject BossFrame;
}
