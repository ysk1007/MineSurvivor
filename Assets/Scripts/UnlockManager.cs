using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserCharacterData //게임 캐릭터 데이터
{
    public bool CharacterAble; // 해금 여부
    public int CharacterLevel;
}

[System.Serializable]
public class UserArtifactData //게임 캐릭터 데이터
{
    public bool ArtifactAble; // 해금 여부
    public bool ArtifactEquip; // 장착 여부
    public int ArtifactLevel;
}

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;
    public bool DataExist = false;

    private string keyNameCha = "UnlockChaData"; //키 값
    private string keyNameArti = "UnlockArtiData"; //키 값
    private string fileName = "UserData.ms"; //파일 이름

    public List<UserCharacterData> OriCha_info; // 오리지널 캐릭터 정보
    public List<UserCharacterData> UserCharacterData;
    public List<UserArtifactData> OriArti_info; // 오리지널 아티팩트 정보
    public List<UserArtifactData> UserArtifactData;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            // 다른 Instance가 존재하면 현재 gameObject를 파괴한다.
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DataSave() //데이터 저장
    {
        for (int i = 0; i < OriCha_info.Count - 1; i++)
        {
            ES3.Save<List<UserCharacterData>>(keyNameCha, UserCharacterData);
        }
        for (int i = 0; i < 3; i++)
        {
            ES3.Save<List<UserArtifactData>>(keyNameArti, UserArtifactData);
        }
    }

    public void DataCreate() //데이터 생성
    {
        for (int i = 0; i < OriCha_info.Count - 1; i++)
        {
            ES3.Save<List<UserCharacterData>>(keyNameCha, OriCha_info);
        }
        for (int i = 0; i < 3; i++)
        {
            ES3.Save<List<UserArtifactData>>(keyNameArti, OriArti_info);
        }
        UserCharacterData = OriCha_info;
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //파일 경로에 데이터 있을 경우
        {
            DataExist = true;
            UserCharacterData = ES3.Load<List<UserCharacterData>>(keyNameCha);
            UserArtifactData = ES3.Load<List<UserArtifactData>>(keyNameArti);
        }
        else
        {
            DataCreate(); //없으면 생성
        }
    }
}
