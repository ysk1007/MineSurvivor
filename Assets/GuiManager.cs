using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public static GuiManager instance;

    [Header(" # 오리지널 캐릭터 데이터")]
    public CharacterData[] characterDatas;

    [Header(" # 오리지널 유물 데이터")]
    public ArtifactData[] ArtifactDatas;

    [Header(" # 캐릭터 선택 해금 오브젝트")]
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    [Header(" # 유물 해금 오브젝트")]
    public GameObject[] lockArtifact;
    public GameObject[] ArtiSlots;

    [Header(" # 업그레이드 팝업")]
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
    public GameObject[] UpgradeBtn;
    public Text NextUpLevel;
    public Text price;
    public int[] UpgradePrice;

    UnlockManager um;
    UserInfoManager ui;

    public GameObject[] CharacterPrefab;

    public Image[] Skill_icons;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        um = UnlockManager.Instance;
        ui = UserInfoManager.Instance;
        CharacterChange(ui.userData.SelectCharacter);
        UnlockCharacter();
        UnlockArtifact();
    }

    private void LateUpdate()
    {
        Setting(ui.userData.SelectCharacter);
    }

    void Setting(int CharacterID)
    {
        CharacterName.text = "레벨 " + (um.UserCharacterData[CharacterID].CharacterLevel + 1) + " " + characterDatas[CharacterID].CharacterName;
        Hp.text = characterDatas[CharacterID].Hp[um.UserCharacterData[CharacterID].CharacterLevel].ToString();
        Damage.text = characterDatas[CharacterID].Damage[um.UserCharacterData[CharacterID].CharacterLevel].ToString();
        ATS.text = characterDatas[CharacterID].ATS;
        Speed.text = characterDatas[CharacterID].Speed;
        Range.text = characterDatas[CharacterID].Range;
        Desc.text = characterDatas[CharacterID].CharacterDesc;
        illust.sprite = characterDatas[CharacterID].illustration;

        if (um.UserCharacterData[CharacterID].CharacterLevel > 3)
        {
            UpDis1.enabled = false;
            UpDis2.enabled = false;
            UpgradeBtn[0].SetActive(false);
            UpgradeBtn[1].SetActive(true);
            NextUpLevel.text = "Lv.Max 업그레이드";
        }
        else
        {
            UpDis1.text = "+" + (characterDatas[CharacterID].Hp[um.UserCharacterData[CharacterID].CharacterLevel + 1] - characterDatas[CharacterID].Hp[um.UserCharacterData[CharacterID].CharacterLevel]).ToString();
            UpDis2.text = "+" + (characterDatas[CharacterID].Damage[um.UserCharacterData[CharacterID].CharacterLevel + 1] - characterDatas[CharacterID].Damage[um.UserCharacterData[CharacterID].CharacterLevel]).ToString();
            price.text = UpgradePrice[um.UserCharacterData[CharacterID].CharacterLevel].ToString();
            NextUpLevel.text = "Lv." + (um.UserCharacterData[CharacterID].CharacterLevel + 2) + " 업그레이드";
        }
        
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
        ui.userData.SelectCharacter = CharacterID;
        for (int i = 0; i < Skill_icons.Length; i++)
        {
            Skill_icons[i].sprite = characterDatas[CharacterID].icons[i];
        }
        ui.DataSave();
    }

    public void ArtiEquip(int index,ArtifactData artifact)
    {
        ArtiSlots[index].GetComponent<Artifact>().data = artifact;
        ArtiSlots[index].GetComponent<Artifact>().Init();
        int[] newEquipSlots = new int[4];
        newEquipSlots[index] = artifact.ArtifactId;
        ui.userData.Equip_Artifacts = newEquipSlots;
        ui.DataSave();
    }

    public void CharacterUpgrade()
    {
        int value = UpgradePrice[um.UserCharacterData[ui.userData.SelectCharacter].CharacterLevel];
        if (ui.userData.GameMoney < value)
            return;
        um.UserCharacterData[ui.userData.SelectCharacter].CharacterLevel++;
        ui.userData.GameMoney -= value;
        ui.DataSave();
        um.DataSave();
    }
}
