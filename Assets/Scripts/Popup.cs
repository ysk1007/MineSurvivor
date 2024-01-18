using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Popup : MonoBehaviour
{
    public Sprite[] Artifact_Frames;
    public Color[] color;
    public GameObject Object;
    public ArtifactData artifact;
    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void ChangePopUp(Popup popup)
    {
        popup.Open();
        popup.artifact = artifact;
        Close();
    }

    public void Equip(int index)
    {
        GuiManager.instance.ArtiEquip(index, artifact);
        Close();
    }

    public void OpenArti(GameObject gameObject)
    {
        ArtifactData data = gameObject.GetComponent<Artifact>().data;
        artifact = data;
        this.gameObject.SetActive(true);
        this.gameObject.GetComponentsInChildren<Image>()[3].sprite = Artifact_Frames[RateIndex(data.Rate)];
        this.gameObject.GetComponentsInChildren<Image>()[4].sprite = data.ArtifactIcon;
        this.gameObject.GetComponentsInChildren<Text>()[0].text = data.ArtifactName;
        this.gameObject.GetComponentsInChildren<Text>()[1].text = RateText(data.Rate);
        this.gameObject.GetComponentsInChildren<Text>()[1].color = color[RateIndex(data.Rate)];
        this.gameObject.GetComponentsInChildren<Text>()[2].text = string.Format(data.ArtifactDesc, data.baseDamge);
        Object.SetActive((UnlockManager.Instance.UserArtifactData[data.ArtifactId].ArtifactAble) == true ? false : true) ;
    }

    public string RateText(ArtifactData.ArtifactRate Rate)
    {
        string ResultStr;

        switch (Rate)
        {
            case ArtifactData.ArtifactRate.NORMAL:
                ResultStr = "일반";
                break;
            case ArtifactData.ArtifactRate.RARE:
                ResultStr = "고급";
                break;
            case ArtifactData.ArtifactRate.UNIQUE:
                ResultStr = "영웅";
                break;
            case ArtifactData.ArtifactRate.LEGENDARY:
                ResultStr = "전설";
                break;
            default:
                ResultStr = "일반";
                break;
        }
        return ResultStr;
    }

    public int RateIndex(ArtifactData.ArtifactRate Rate)
    {
        int index;

        switch (Rate)
        {
            case ArtifactData.ArtifactRate.NORMAL:
                index = 0;
                break;
            case ArtifactData.ArtifactRate.RARE:
                index = 1;
                break;
            case ArtifactData.ArtifactRate.UNIQUE:
                index = 2;
                break;
            case ArtifactData.ArtifactRate.LEGENDARY:
                index = 3;
                break;
            default:
                index = 0;
                break;
        }
        return index;
    }
}
