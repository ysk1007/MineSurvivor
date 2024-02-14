using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserData //���� ������ Ŭ����
{
    public string UserName; //�г���
    public string UserId; //���� ���̵�
    public int GameMoney; // ��� ��ȭ
    public int GameGem; // ���� ��ȭ
    public int SelectCharacter; // ���õ� ĳ����
    public int[] Equip_Artifacts; // ���� �ϰ� �ִ� ��Ƽ��Ʈ �ѹ�
    public string LastPlayTime; // ������ ���� �ð�
    public int PlayCounter; // ���� ī��Ʈ
    public int Attendance; // �⼮��
    public bool TodayStamp; // ���� �⼮üũ ����

    public int TodayProgress; // ���� Ȱ�൵
    public int[] TodayMission; // ���� �̼� Ŭ���� ����
    public int[] TodayMissionValue; // ���� �̼� ���� ��Ȳ
    public int[] TodayReward; // ���� �̼� ����

    public int WeekProgress; // ���� Ȱ�൵
    public int[] WeekMission; // �ְ� �̼� Ŭ���� ����
    public int[] WeekMissionValue; // ���� �̼� ���� ��Ȳ
    public int[] WeekReward; // �ְ� �̼� ����
}

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance;
    public UnlockManager unlockManager;
    public bool DataExist = false;

    private string keyName = "UserData"; //Ű ��
    private string fileName = "UserData.ms"; //���� �̸�

    public UserData userData;
    // ������ ������ �ð��� ������ ����
    private DateTime LastPlayTime;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            // �ٸ� Instance�� �����ϸ� ���� gameObject�� �ı��Ѵ�.
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DataLoad();
            DontDestroyOnLoad(gameObject);
            NextDay();
        }
    }

    public void DataCreate() //������ ����
    {
        TodayMissionReset();
        WeekMissionReset();

        userData.Equip_Artifacts = new int[4];
        userData.LastPlayTime = KoreaDate();
        ES3.Save(keyName, userData);
    }

    

    public void DataSave() //������ ����
    {
        //userData.LastPlayTime = KoreaDate();
        ES3.Save(keyName, userData);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //���� ��ο� ������ ���� ���
        {
            DataExist = true;
            ES3.LoadInto(keyName, userData); //�ε�
            unlockManager.DataLoad();
        }
        else
        {
            DataCreate(); //������ ����
            unlockManager.DataCreate();
        }
    }

    public void Stamp()
    {
        if (userData.TodayStamp)
            return;
        userData.TodayStamp = true;
        userData.TodayMission[0]++;
        userData.LastPlayTime = KoreaDate();
        DailyReward();
        ES3.Save(keyName, userData);
    }

    void NextDay()
    {
        // ������ ������ �ð� �������� (ù ���� �ÿ��� �⺻������ ���� �ð� ����)
        string savedTime = userData.LastPlayTime;
        LastPlayTime = string.IsNullOrEmpty(savedTime) ? DateTime.UtcNow : DateTime.Parse(savedTime);

        // ���� �ð� ��������
        DateTime currentTime = DateTime.UtcNow;

        // ���ѹα� �ð���� ��ȯ
        TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        DateTime koreanCurrentTime = TimeZoneInfo.ConvertTime(currentTime, koreaTimeZone);


        // ������ ������ �ð��� ���� �ð� ���� �ϼ� ���� ���
        TimeSpan timeDifference = koreanCurrentTime.Date - LastPlayTime.Date;
        int daysPassed = timeDifference.Days;
        userData.PlayCounter += daysPassed;

        // ������ ������ �ð��� ���ú��� �������� Ȯ��
        if (LastPlayTime.Date < koreanCurrentTime.Date)
        {
            // �Ϸ簡 ������
            Debug.Log("�Ϸ簡 �������ϴ�.");

            if (userData.TodayStamp) // ���� �⼮ ������ �޾��� ���
                userData.Attendance++;

            if (userData.Attendance > 6) // �⼮ ������ ��� ����
                userData.Attendance = 0;

            TodayMissionReset(); // ���� �̼� �ʱ�ȭ
            if (userData.PlayCounter % 7 == 0) // ������ ī��Ʈ
                WeekMissionReset();

            userData.TodayStamp = false;
            GuiManager.instance.DailyReward.newDay = true;
        }
        else
        {
            Debug.Log("���� �Դϴ�.");
        }

        DataSave();
    }

    string KoreaDate()
    {
        // ���� �ð� ��������
        DateTime currentTime = DateTime.UtcNow;

        // ���ѹα� �ð���� ��ȯ
        TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        DateTime koreanCurrentTime = TimeZoneInfo.ConvertTime(currentTime, koreaTimeZone);
        return koreanCurrentTime.ToString();
    }

    void DailyReward()
    {
        switch (userData.Attendance)
        {
            case 0:
                userData.GameMoney += 5000;
                RewardManager.instance.CollectReward(0, 5000);
                break;
            case 1:
                userData.GameMoney += 1;
                break;
            case 2:
                userData.GameGem += 100;
                RewardManager.instance.CollectReward(1, 100);
                break;
            case 3:
                userData.GameMoney += 10000;
                RewardManager.instance.CollectReward(0, 10000);
                break;
            case 4:
                userData.GameMoney += 2;
                break;
            case 5:
                userData.GameGem += 200;
                RewardManager.instance.CollectReward(1, 200);
                break;
            case 6:
                userData.GameGem += 1000;
                break;
        }

    }

    public void MissionComplite(int type, int index) // 0 : ����, 1 : �ְ�
    {
        MissionReward(type,index);
        ES3.Save(keyName, userData);
    }

    void MissionReward(int type, int index)
    {
        if (type == 0)
        {
            switch (index)
            {
                case 0:
                    userData.GameMoney += 500;
                    RewardManager.instance.CollectReward(0, 500);
                    break;
                case 1:
                    userData.GameMoney += 1;
                    RewardManager.instance.CollectReward(0, 1);
                    break;
                case 2:
                    userData.GameMoney += 1;
                    RewardManager.instance.CollectReward(0, 1);
                    break;
                case 3:
                    userData.GameGem += 50;
                    RewardManager.instance.CollectReward(1, 50);
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    userData.GameMoney += 8000;
                    RewardManager.instance.CollectReward(0, 8000);
                    break;
                case 1:
                    userData.GameMoney += 5;
                    RewardManager.instance.CollectReward(0, 5);
                    break;
                case 2:
                    userData.GameMoney += 1;
                    RewardManager.instance.CollectReward(0, 1);
                    break;
                case 3:
                    userData.GameGem += 1;
                    RewardManager.instance.CollectReward(1, 1);
                    break;
            }
        }
    }

    void TodayMissionReset()
    {
        userData.TodayProgress = 0;
        userData.TodayMission = new int[6];
        userData.TodayMissionValue = new int[6];
        userData.TodayReward = new int[4];
    }
    private void WeekMissionReset()
    {
        userData.WeekProgress = 0;
        userData.WeekMission = new int[5];
        userData.WeekMissionValue = new int[5];
        userData.WeekReward = new int[4];
    }
}
