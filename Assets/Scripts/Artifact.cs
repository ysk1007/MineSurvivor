using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public ArtifactData data;
    public Sprite[] frames;
    public Sprite Null_img;
    public Image icon;
    void Awake()
    {
        this.icon = GetComponentsInChildren<Image>()[1];
        Init();
    }

    public void Init()
    {
        if (!data)
        {
            this.icon.sprite = Null_img;
            this.gameObject.GetComponent<Image>().sprite = Null_img;
        }
        else
        {
            this.icon = GetComponentsInChildren<Image>()[1];
            this.icon.sprite = data.ArtifactIcon;
            this.gameObject.GetComponent<Image>().sprite = frames[data.Rate.GetHashCode()];
        }
    }
}
