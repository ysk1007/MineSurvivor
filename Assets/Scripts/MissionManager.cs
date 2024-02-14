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

    [Header(" # 활약도 보상 버튼")]
    public Button[] TodayRewardBtn; // 금일 활약도 보상 버튼
    public Button[] WeekRewardBtn; // 주간 활약도 보상 버튼

    public Color[] ColorList;
    public bool isReward;
    UserInfoManager um;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        um = UserInfoManager.Instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < TodaySlider.Length; i++)
        {
            TodaySlider[i].value = um.userData.TodayMissionValue[i];
            if (TodaySlider[i].maxValue == um.userData.TodayMissionValue[i])
                TodayMissionBtn[i].SetActive(true);
        }
        for (int i = 0; i < WeekSlider.Length; i++)
        {
            WeekSlider[i].value = um.userData.WeekMissionValue[i];
            if (WeekSlider[i].maxValue == um.userData.WeekMissionValue[i])
                WeekMissionBtn[i].SetActive(true);
        }
    }
}
