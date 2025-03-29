using System;
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
    public int GameMoney; // 골드 재화
    public int GameGem; // 보석 재화
    public int SelectCharacter; // 선택된 캐릭터
    public int[] Equip_Artifacts; // 장착 하고 있는 아티팩트 넘버
    public string LastPlayTime; // 마지막 접속 시간
    public int PlayCounter; // 접속 카운트
    public int Attendance; // 출석일
    public bool TodayStamp; // 금일 출석체크 여부

    public int TodayProgress; // 금일 활약도
    public int[] TodayMission; // 금일 미션 클리어 여부
    public int[] TodayMissionValue; // 금일 미션 진행 상황
    public int[] TodayReward; // 금일 미션 보상

    public int WeekProgress; // 금일 활약도
    public int[] WeekMission; // 주간 미션 클리어 여부
    public int[] WeekMissionValue; // 금일 미션 진행 상황
    public int[] WeekReward; // 주간 미션 보상
}

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance;
    public UnlockManager unlockManager;
    public bool DataExist = false;

    private string keyName = "UserData"; //키 값
    private string fileName = "UserData.ms"; //파일 이름

    public UserData userData;
    // 이전에 저장한 시간을 보관할 변수
    private DateTime LastPlayTime;

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
            NextDay();
        }
    }

    public void DataCreate() //데이터 생성
    {
        TodayMissionReset();
        WeekMissionReset();

        userData.Equip_Artifacts = new int[4];
        userData.LastPlayTime = KoreaDate();
        ES3.Save(keyName, userData);
    }

    

    public void DataSave() //데이터 저장
    {
        //userData.LastPlayTime = KoreaDate();
        ES3.Save(keyName, userData);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //파일 경로에 데이터 있을 경우
        {
            DataExist = true;
            ES3.LoadInto(keyName, userData); //로드
            unlockManager.DataLoad();
        }
        else
        {
            DataCreate(); //없으면 생성
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
        // 이전에 저장한 시간 가져오기 (첫 실행 시에는 기본값으로 현재 시간 설정)
        string savedTime = userData.LastPlayTime;
        LastPlayTime = string.IsNullOrEmpty(savedTime) ? DateTime.UtcNow : DateTime.Parse(savedTime);

        // 현재 시간 가져오기
        DateTime currentTime = DateTime.UtcNow;

        // 대한민국 시간대로 변환
        TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        DateTime koreanCurrentTime = TimeZoneInfo.ConvertTime(currentTime, koreaTimeZone);


        // 이전에 저장한 시간과 현재 시간 간의 일수 차이 계산
        TimeSpan timeDifference = koreanCurrentTime.Date - LastPlayTime.Date;
        int daysPassed = timeDifference.Days;
        userData.PlayCounter += daysPassed;

        // 이전에 저장한 시간이 오늘보다 이전인지 확인
        if (LastPlayTime.Date < koreanCurrentTime.Date)
        {
            // 하루가 지났음
            Debug.Log("하루가 지났습니다.");

            if (userData.TodayStamp) // 어제 출석 보상을 받았을 경우
                userData.Attendance++;

            if (userData.Attendance > 6) // 출석 보상을 모두 받음
                userData.Attendance = 0;

            TodayMissionReset(); // 일일 미션 초기화
            if (userData.PlayCounter % 7 == 0) // 일주일 카운트
                WeekMissionReset();

            userData.TodayStamp = false;
            GuiManager.instance.DailyReward.newDay = true;
        }
        else
        {
            Debug.Log("아직 입니다.");
        }

        DataSave();
    }

    string KoreaDate()
    {
        // 현재 시간 가져오기
        DateTime currentTime = DateTime.UtcNow;

        // 대한민국 시간대로 변환
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

    public void MissionComplite(int type, int index) // 0 : 금일, 1 : 주간
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
