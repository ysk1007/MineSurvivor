using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Popup : MonoBehaviour
{
    public Sprite[] Artifact_Frames;
    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenArti(ArtifactData data)
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponentsInChildren<Image>()[3].sprite = data.ArtifactIcon;
        this.gameObject.GetComponentsInChildren<Text>()[0].text = data.ArtifactName;
        this.gameObject.GetComponentsInChildren<Text>()[1].text = string.Format(data.ArtifactDesc, data.baseDamge);
    }
}
