using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public static GuiManager instance;

    public CharacterData[] characterDatas;
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    public GameObject[] lockArtifact;
    public GameObject[] ArtiSlots;

    public Text CharacterName;
    public Text Hp;
    public Text Damage;
    public Text ATS;
    public Text Speed;
    public Text Range;
    public Text Desc;
    public Text UpDis1;
    public Text UpDis2;
    public Image illust;
    UnlockManager um;

    public GameObject[] CharacterPrefab;

    public Image[] Skill_icons;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        um = UnlockManager.Instance;
        CharacterChange(UserInfoManager.Instance.userData.SelectCharacter);
        UnlockCharacter();
        UnlockArtifact();
    }

    private void LateUpdate()
    {
        Setting(UserInfoManager.Instance.userData.SelectCharacter);
    }

    void Setting(int CharacterID)
    {
        CharacterName.text = "·¹º§ " + (characterDatas[CharacterID].CharacterLevel + 1) + " " + characterDatas[CharacterID].CharacterName;
        Hp.text = characterDatas[CharacterID].Hp[um.UserCharacterData[CharacterID].CharacterLevel].ToString();
        Damage.text = characterDatas[CharacterID].Damage[um.UserCharacterData[CharacterID].CharacterLevel].ToString();
        ATS.text = characterDatas[CharacterID].ATS;
        Speed.text = characterDatas[CharacterID].Speed;
        Range.text = characterDatas[CharacterID].Range;
        Desc.text = characterDatas[CharacterID].CharacterDesc;
        illust.sprite = characterDatas[CharacterID].illustration;
        UpDis1.text = "+" + (characterDatas[CharacterID].Hp[um.UserCharacterData[CharacterID].CharacterLevel + 1] - characterDatas[CharacterID].Hp[um.UserCharacterData[CharacterID].CharacterLevel]).ToString();
        UpDis2.text = "+" + (characterDatas[CharacterID].Damage[um.UserCharacterData[CharacterID].CharacterLevel + 1] - characterDatas[CharacterID].Damage[um.UserCharacterData[CharacterID].CharacterLevel]).ToString();
    }

    public void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            lockCharacter[i].SetActive(!um.UserCharacterData[i + 1].CharacterAble);
            unlockCharacter[i].SetActive(um.UserCharacterData[i + 1].CharacterAble);
        }
    }

    public void UnlockArtifact()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            lockArtifact[i].SetActive(!um.UserArtifactData[i].ArtifactAble);
        }
    }

    public void CharacterChange(int CharacterID)
    {
        UserInfoManager.Instance.userData.SelectCharacter = CharacterID;
        for (int i = 0; i < Skill_icons.Length; i++)
        {
            Skill_icons[i].sprite = characterDatas[CharacterID].icons[i];
        }
    }

    public void ArtiEquip(int index,ArtifactData artifact)
    {
        ArtiSlots[index].GetComponent<Artifact>().data = artifact;
        ArtiSlots[index].GetComponent<Artifact>().Init();
    }
}
