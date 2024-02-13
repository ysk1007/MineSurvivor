using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] Tab;
    public Image[] TabBtnImage;
    public Sprite IdleSprite, SelectSprite;

    // Start is called before the first frame update
    void Start()
    {
        TabClick(2);
    }

    public void TabClick(int n) {
        for (int i = 0; i < Tab.Length; i++)
        {   
            //if문으로 true,false 하는것을 축약
            Tab[i].SetActive(i == n);
            TabBtnImage[i].sprite = i == n ? SelectSprite : IdleSprite;
        }
        if (n == 1)
        {
            GuiManager.instance.UnlockArtifact();
        }
    }
}
