using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserData //유저 데이터 클래스
{
    public string UserName; //닉네임
    public string UserId; //고유 아이디
    public int GameMoney;
    public int GameGem;
    public int SelectCharacter;
}

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance;
    public bool DataExist = false;

    private string keyName = "UserData"; //키 값
    private string fileName = "UserData.ms"; //파일 이름

    public UserData userData;

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
            DataLoad();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DataSave() //데이터 생성
    {
        ES3.Save(keyName, userData);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //파일 경로에 데이터 있을 경우
        {
            DataExist = true;
            ES3.LoadInto(keyName, userData); //로드
            UnlockManager.Instance.DataLoad();
        }
        else
        {
            DataSave(); //없으면 생성
            UnlockManager.Instance.DataCreate();
        }
    }
}
