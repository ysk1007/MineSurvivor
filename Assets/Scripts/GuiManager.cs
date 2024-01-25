using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental;
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
    public Image[] EquipFrame;

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

    [Header(" # 뽑기 화면 관련")]
    public Animator ChestAnim;
    public List<ArtifactData> ChestResult;
    public GameObject ArtiList;
    public Artifact[] Artifactx10;
    bool GetSpecialArti;

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
        ArtiSetting();
        Artifactx10 = ArtiList.GetComponentsInChildren<Artifact>();
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
        int[] newEquipSlots = ui.userData.Equip_Artifacts;

        ArtiSlots[index].GetComponent<Artifact>().data = artifact;
        ArtiSlots[index].GetComponent<Artifact>().Init();

        newEquipSlots[index] = artifact.ArtifactId;

        ui.userData.Equip_Artifacts = newEquipSlots;

        um.UserArtifactData[artifact.ArtifactId - 1].ArtifactEquip = true;

        ui.DataSave();
        um.DataSave();
        EquipFrame[artifact.ArtifactId - 1].enabled = true;
    }

    public void ArtiUnEquip(int ArtifactId)
    {
        int[] newEquipSlots = ui.userData.Equip_Artifacts;
        for (int i = 0; i < newEquipSlots.Length; i++)
        {
            if (newEquipSlots[i] == ArtifactId)
            {
                newEquipSlots[i] = 0;
                um.UserArtifactData[ArtifactId - 1].ArtifactEquip = false;
                ArtiSlots[i].GetComponent<Artifact>().data = null;
                ArtiSlots[i].GetComponent<Artifact>().Init();
                break;
            }
        }

        ui.DataSave();
        um.DataSave();

        EquipFrame[ArtifactId - 1].enabled = false;
    }

    public void ArtiSetting()
    {
        for (int i = 0; i < ui.userData.Equip_Artifacts.Length; i++)
        {
            if (ui.userData.Equip_Artifacts[i] == 0)
                continue;
            ArtiSlots[i].GetComponent<Artifact>().data = ArtifactDatas[ui.userData.Equip_Artifacts[i] - 1];
            ArtiSlots[i].GetComponent<Artifact>().Init();
            EquipFrame[ui.userData.Equip_Artifacts[i] - 1].enabled = true;
        }
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

    public void ChestOpen()
    {
        if (GetSpecialArti)
        {
            ChestAnim.SetTrigger("Special");
        }
        else
        {
            ChestAnim.SetTrigger("Open");
        }
    }

    public void ChestSkip()
    {
        ChestAnim.SetTrigger("Skip");
    }

    public void RandomArtifact()
    {
        GetSpecialArti = false;
        int[] normal = { 0 };
        int[] rare = { 1 };
        int[] unique = { 2 };
        int[] legendary = { 3 };
        Random.Range(0, ArtifactDatas.Length);

        for (int i = 0; i < 9; i++) // 10연차
        {
            int index;
            switch (GenerateRandomValue())
            {
                case 0:
                    index = normal[(Random.Range(0, normal.Length))];
                    break;
                case 1:
                    index = rare[(Random.Range(0, rare.Length))];
                    break;
                case 2:
                    index = unique[(Random.Range(0, unique.Length))];
                    GetSpecialArti = true;
                    break;
                case 3:
                    index = legendary[(Random.Range(0, legendary.Length))];
                    GetSpecialArti = true;
                    break;
                default:
                    index = 0;
                    break;
            }
            ChestResult.Add(ArtifactDatas[index]);
        }

        for (int i = 0; i < 9; i++)
        {
            Artifactx10[i].data = ChestResult[i];
            Artifactx10[i].Init();
        }
    }

    public int GenerateRandomValue()
    {
        float randomValue = Random.Range(0f, 100f); // 0에서 100 사이의 난수 생성

        if (randomValue < 74f)
        {
            return 0;
        }
        else if (randomValue < 89.3f)
        {
            return 1;
        }
        else if (randomValue < 98.4f)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}
