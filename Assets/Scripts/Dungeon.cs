using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Dungeon : MonoBehaviour
{
    public DungeonData data;
    Image MapBgSprite;
    Image BossSprite;
    Text DungeonTitle;
    void Awake()
    {
        this.MapBgSprite = GetComponentsInChildren<Image>()[1];
        this.BossSprite = GetComponentsInChildren<Image>()[2];
        this.DungeonTitle = GetComponentsInChildren<Text>()[0];
        Init();
    }

    public void Init()
    {
        if (!data)
            return;
        else
        {
            this.MapBgSprite.sprite = data.MapBgSprite;
            this.BossSprite.sprite = data.BossSprite;
            this.DungeonTitle.text = data.DungeonName;
        }
    }
}
