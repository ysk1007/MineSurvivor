using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    public Text CharacterName;
    public Text Hp;
    public Text Damage;
    public Text ATS;
    public Text Speed;
    public Text Range;
    UnlockManager um;

    public GameObject[] CharacterPrefab;
    public GameObject[] Popup_CharacterPrefab;

    public Image[] Skill_icons;
    public Sprite[] icon_prefabs_0, icon_prefabs_1;

    private void Awake()
    {
        um = UnlockManager.Instance;
    }

    public void Start()
    {
        CharacterChange(UserInfoManager.Instance.userData.SelectCharacter);
        UnlockCharacter();
    }

    private void LateUpdate()
    {
        Setting(UserInfoManager.Instance.userData.SelectCharacter);
    }

    void Setting(int CharacterID)
    {
        CharacterName.text = "·¹º§ " + (um.UserUnlockData[CharacterID].CharacterLevel + 1) + " " + um.OriCha_info[CharacterID].CharacterName;
        Hp.text = um.OriCha_info[CharacterID].Hp[um.UserUnlockData[CharacterID].CharacterLevel].ToString();
        Damage.text = um.OriCha_info[CharacterID].Damage[um.UserUnlockData[CharacterID].CharacterLevel].ToString();
        ATS.text = um.OriCha_info[CharacterID].ATS;
        Speed.text = um.OriCha_info[CharacterID].Speed;
        Range.text = um.OriCha_info[CharacterID].Range;
    }

    public void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            lockCharacter[i].SetActive(!um.UserUnlockData[i + 1].CharacterAble);
            unlockCharacter[i].SetActive(um.UserUnlockData[i + 1].CharacterAble);
        }
    }

    public void CharacterChange(int CharacterID)
    {
        UserInfoManager.Instance.userData.SelectCharacter = CharacterID;
        for (int i = 0; i < CharacterPrefab.Length; i++)
        {
            CharacterPrefab[i].SetActive((CharacterID == i) ? true : false);
            Popup_CharacterPrefab[i].SetActive((CharacterID == i) ? true : false);
        }

        switch (CharacterID)
        {
            case 0:
                for (int i = 0; i < Skill_icons.Length; i++)
                {
                    Skill_icons[i].sprite = icon_prefabs_0[i];
                }
                break;
            case 1:
                for (int i = 0; i < Skill_icons.Length; i++)
                {
                    Skill_icons[i].sprite = icon_prefabs_1[i];
                }
                break;
        }
    }
}
