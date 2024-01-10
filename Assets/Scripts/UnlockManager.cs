using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData //게임 캐릭터 데이터
{
    public string CharacterName; // 이름
    public int CharacterID; // 고유 번호
    public bool CharacterAble; // 해금 여부
    public int CharacterLevel;
    public int[] Damage; // 피해량
    public int[] Hp; // 최대 체력
    public string ATS; // 공격 속도
    public string Speed; // 이동 속도
    public string Range; // 사거리
}

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;
    public bool DataExist = false;

    private string keyName = "UnlockData"; //키 값
    private string fileName = "UserData.ms"; //파일 이름

    public List<CharacterData> OriCha_info; // 오리지널 캐릭터 정보
    public List<CharacterData> UserUnlockData;

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
            ES3.Save<List<CharacterData>>(keyName, UserUnlockData);
        }
    }

    public void DataCreate() //데이터 생성
    {
        for (int i = 0; i < OriCha_info.Count - 1; i++)
        {
            ES3.Save<List<CharacterData>>(keyName, OriCha_info);
        }
        UserUnlockData = OriCha_info;
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //파일 경로에 데이터 있을 경우
        {
            DataExist = true;
            UserUnlockData = ES3.Load<List<CharacterData>>(keyName);
        }
        else
        {
            DataCreate(); //없으면 생성
        }
    }
}
