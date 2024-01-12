using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public ArtifactData data;
    Image icon;
    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.ArtifactIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
