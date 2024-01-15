using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public ArtifactData data;
    public Sprite[] frames;
    Image icon;
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (!data)
            return;
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.ArtifactIcon;
        this.gameObject.GetComponent<Image>().sprite = frames[data.Rate.GetHashCode()];
    }
}
