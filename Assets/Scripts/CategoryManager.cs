using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    public GameObject[] Tab;
    public Image[] TabBtnImage;
    public TextMeshProUGUI[] Texts;
    public Color idleColor, SelectColor;

    // Start is called before the first frame update
    void Start()
    {
        CategoryClick(0);
    }

    public void CategoryClick(int n)
    {
        for (int i = 0; i < Tab.Length; i++)
        {
            //if문으로 true,false 하는것을 축약
            Tab[i].SetActive(i == n);
            TabBtnImage[i].enabled = i == n ? true : false;
            Texts[i].color = i == n ? SelectColor : idleColor;
        }
    }
}
