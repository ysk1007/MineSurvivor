using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Popup : MonoBehaviour
{
    public Sprite[] Artifact_Frames;
    public Color[] color;
    public GameObject[] Object;
    public ArtifactData artifact;
    public Button button;
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

    public void ChestPopOpen()
    {
        this.gameObject.transform.localScale = Vector3.one;
        button.enabled = true;
    }

    public void ChestPopClose()
    {
        this.gameObject.transform.localScale = Vector3.zero;
    }

    public void Equip(int index)
    {
        GuiManager.instance.ArtiEquip(index, artifact);
        Close();
    }

    public void UnEquip()
    {
        GuiManager.instance.ArtiUnEquip(artifact.ArtifactId);
        Close();
    }

    public void OpenArti(GameObject gameObject)
    {
        if (!gameObject.GetComponent<Artifact>().data)
            return;
        ArtifactData data = gameObject.GetComponent<Artifact>().data;
        artifact = data;
        this.gameObject.SetActive(true);
        this.gameObject.GetComponentsInChildren<Image>()[3].sprite = Artifact_Frames[RateIndex(data.Rate)];
        this.gameObject.GetComponentsInChildren<Image>()[4].sprite = data.ArtifactIcon;
        this.gameObject.GetComponentsInChildren<Text>()[0].text = data.ArtifactName;
        this.gameObject.GetComponentsInChildren<Text>()[1].text = RateText(data.Rate);
        this.gameObject.GetComponentsInChildren<Text>()[1].color = color[RateIndex(data.Rate)];
        this.gameObject.GetComponentsInChildren<Text>()[2].text = string.Format(data.ArtifactDesc, data.baseDamge[data.ArtiLevel]);

        int maxValue = GuiManager.instance.ArtifactUpgradeExp[UnlockManager.Instance.UserArtifactData[data.ArtifactId - 1].ArtifactLevel];
        int CurExp = UnlockManager.Instance.UserArtifactData[data.ArtifactId - 1].ArtifactExp;

        this.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = (UnlockManager.Instance.UserArtifactData[data.ArtifactId - 1].ArtifactLevel > 3) ? "MAX" : CurExp.ToString() + "/" + maxValue.ToString();
        this.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = (UnlockManager.Instance.UserArtifactData[data.ArtifactId - 1].ArtifactLevel + 1).ToString();
        this.gameObject.GetComponentsInChildren<Slider>()[0].maxValue = maxValue;
        this.gameObject.GetComponentsInChildren<Slider>()[0].value = CurExp;

        bool isEquip = (UnlockManager.Instance.UserArtifactData[data.ArtifactId - 1].ArtifactEquip);
        bool isAble = (UnlockManager.Instance.UserArtifactData[data.ArtifactId - 1].ArtifactAble);

        // 0 장착, 1 해제, 2 미보유 버튼
        if (!isAble)
        {
            Object[0].SetActive(false);
            Object[1].SetActive(false);

            Object[2].SetActive(true);
        }
        else
        {
            Object[2].SetActive(false);
            if (isEquip) //장착 중
            {
                Object[0].SetActive(false);
                Object[1].SetActive(true);

            }
            else if (!isEquip)
            {
                Object[0].SetActive(true);
                Object[1].SetActive(false);

            }   
        }
    }

    public void LevelPopupSetting()
    {
        int CharacterId = UserInfoManager.Instance.userData.SelectCharacter;
        int Level = UnlockManager.Instance.UserCharacterData[CharacterId].CharacterLevel;
        this.gameObject.SetActive(true);
        this.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = (Level + 1).ToString();

        this.gameObject.GetComponentsInChildren<Text>()[2].text = GuiManager.instance.characterDatas[CharacterId].Hp[Level - 1].ToString();
        this.gameObject.GetComponentsInChildren<Text>()[3].text = GuiManager.instance.characterDatas[CharacterId].Hp[Level].ToString();

        this.gameObject.GetComponentsInChildren<Text>()[5].text = GuiManager.instance.characterDatas[CharacterId].Damage[Level - 1].ToString();
        this.gameObject.GetComponentsInChildren<Text>()[6].text = GuiManager.instance.characterDatas[CharacterId].Damage[Level].ToString();
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
