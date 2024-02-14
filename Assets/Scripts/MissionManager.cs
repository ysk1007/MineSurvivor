using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [Header(" # �����̴�")]
    public Slider[] TodaySlider;
    public Slider[] WeekSlider;

    [Header(" # �̼� ���൵ �ؽ�Ʈ")]
    public TextMeshProUGUI[] TodaySliderValue;
    public TextMeshProUGUI[] WeekSliderValue;

    [Header(" # Ȱ�൵ Value �ؽ�Ʈ")]
    public TextMeshProUGUI TodayProgressValue;
    public TextMeshProUGUI WeekProgressValue;

    [Header(" # �̼� ���� �ޱ� ��ư")]
    public GameObject[] TodayMissionBtn;
    public GameObject[] WeekMissionBtn;

    [Header(" # Ȱ�൵ ���� ��ư")]
    public Button[] TodayRewardBtn; // ���� Ȱ�൵ ���� ��ư
    public Button[] WeekRewardBtn; // �ְ� Ȱ�൵ ���� ��ư

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
