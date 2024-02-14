using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [Header(" # 슬라이더")]
    public Slider[] TodaySlider;
    public Slider[] WeekSlider;

    [Header(" # 미션 진행도 텍스트")]
    public TextMeshProUGUI[] TodaySliderValue;
    public TextMeshProUGUI[] WeekSliderValue;

    [Header(" # 활약도 Value 텍스트")]
    public TextMeshProUGUI TodayProgressValue;
    public TextMeshProUGUI WeekProgressValue;

    [Header(" # 미션 보상 받기 버튼")]
    public GameObject[] TodayMissionBtn;
    public GameObject[] WeekMissionBtn;

    [Header(" # 활약도 슬라이더")]
    public Slider TodayRewardSlider;
    public Slider WeekRewardSlider;

    [Header(" # 활약도 보상 버튼")]
    public Button[] TodayRewardBtn; // 금일 활약도 보상 버튼
    public Button[] WeekRewardBtn; // 주간 활약도 보상 버튼

    [Header(" # 활약도 보상 포커스 이미지")]
    public Image[] TodayRewardFocus; // 금일 활약도 보상 포커스 이미지
    public Image[] WeekRewardFocus; // 주간 활약도 보상 포커스 이미지

    public Color[] ColorList;
    public bool isReward;
    public Text ResetText;
    UserInfoManager um;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        um = UserInfoManager.Instance;
        ResetText.text = (7 - (um.userData.PlayCounter % 7)).ToString() + "일 후 초기화 됩니다.";
    }

    private void LateUpdate()
    {
        RewardBtnSetting();
        ProgressRewardSetting();
    }

    public void TodayRewardButton(int index) // 미션 보상 받기 버튼 기능
    {
        RewardManager.instance.CollectReward(2,0);
        um.userData.TodayMission[index] = 1;
        switch (index)
        {
            case 0:
                um.userData.TodayProgress += 10;
                break;
            case 1:
                um.userData.TodayProgress += 10;
                break;
            case 2:
                um.userData.TodayProgress += 15;
                break;
            case 3:
                um.userData.TodayProgress += 20;
                break;
            case 4:
                um.userData.TodayProgress += 25;
                break;
            case 5:
                um.userData.TodayProgress += 20;
                break;
        }
        um.DataSave();
    }

    public void WeekRewardButton(int index) // 미션 보상 받기 버튼 기능
    {
        RewardManager.instance.CollectReward(2, 0);
        um.userData.WeekMission[index] = 1;
        um.userData.WeekProgress += 20;
        um.DataSave();
    }

    void RewardBtnSetting() // 보상 받기 버튼 활성화 여부 메소드
    {
        for (int i = 0; i < TodaySlider.Length; i++)
        {
            if (um.userData.TodayMission[i] == 1)
                TodayMissionBtn[i].SetActive(false);
            else
            {
                if (TodaySlider[i].maxValue == um.userData.TodayMissionValue[i])
                    TodayMissionBtn[i].SetActive(true);
            }
        }
        for (int i = 0; i < WeekSlider.Length; i++)
        {
            if (um.userData.WeekMission[i] == 1)
                WeekMissionBtn[i].SetActive(false);

            else
            {
                if (WeekSlider[i].maxValue == um.userData.WeekMissionValue[i])
                    WeekMissionBtn[i].SetActive(true);
            }
        }
    }

    void ProgressRewardSetting() // 활약도 보상 체크 여부 메소드
    {
        for (int i = 0; i < TodayRewardFocus.Length; i++)
        {
            if (TodayRewardSlider.value >= (i + 1) * 25)
            {
                TodayRewardFocus[i].enabled = true;
                TodayRewardBtn[i].enabled = (um.userData.TodayReward[i] != 1) ? true : false;
            }
            else
            {
                TodayRewardFocus[i].enabled = false;
                TodayRewardBtn[i].enabled = false;
            }
        }

        for (int i = 0; i < WeekRewardFocus.Length; i++)
        {
            if (WeekRewardSlider.value >= (i + 1) * 25)
            {
                WeekRewardFocus[i].enabled = true;
                WeekRewardBtn[i].enabled = (um.userData.WeekReward[i] != 1) ? true : false;
            }
            else
            {
                WeekRewardFocus[i].enabled = false;
                WeekRewardBtn[i].enabled = false;
            }
        }
    }

    public void TodayProgressRewardButton(int index) // 금일 활약도 보상 버튼 기능
    {
        um.userData.TodayReward[index] = 1;
        um.MissionComplite(0,index);
    }

    public void WeekProgressRewardButton(int index) // 주간 활약도 보상 버튼 기능
    {
        um.userData.WeekReward[index] = 1;
        um.MissionComplite(1,index);
    }
}
