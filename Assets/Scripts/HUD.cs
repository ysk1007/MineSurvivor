using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InfoType { 
        Exp, Level, Kill, Timer, Health, 
        IronCount, GoldCount, DiamondCount, 
        HealthText, ExpText, ChargeBar, 
        UserGold, UserGem, UserName,
        DailyNotice, MissionNotice, 
        TodayProgress, WeekProgress, // 활약도
        TodaySlider, WeekSlider, // 슬라이더
        TodayRewardSlider, WeekRewardSlider, // 보상 슬라이더
        TodayMissionValue, WeekMissionValue, // 텍스트 Value
        TodayMissionImage, WeekMissionImage,
        TodayClearFocus, WeekClearFocus
    }

    public int index;
    public InfoType type;
    public Image image;

    Text thisText;
    Slider thisSlider;
    TextMeshProUGUI TextMeshProUGUI;
    Image thisimage;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        thisSlider = GetComponent<Slider>();
        TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        thisimage = GetComponent<Image>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level,GameManager.instance.nextExp.Length - 1)];
                thisSlider.value = curExp / maxExp;
                break;
            case InfoType.ExpText:
                float cExp = GameManager.instance.exp;
                float mExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                thisText.text = string.Format("{0:F0}%", cExp / mExp * 100); // Format 0은 들어올 첫 번째 인자 값의 위치
                break;
            case InfoType.Level:
                thisText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); // Format 0은 들어올 첫 번째 인자 값의 위치
                break;
            case InfoType.Kill:
                thisText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Timer:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                thisText.text = string.Format("{0:D2}:{1:D2}", min, sec); // D는 고정 자리수
                break;
            case InfoType.Health:
                float curHp = GameManager.instance.curHp;
                float maxHp = GameManager.instance.maxHp;
                thisSlider.value = curHp / maxHp;
                break;
            case InfoType.HealthText:
                float Hp = GameManager.instance.curHp / GameManager.instance.maxHp * 100;
                thisText.text = string.Format("{0:F0}%", Hp); // Format 0은 들어올 첫 번째 인자 값의 위치
                thisText.color = color();
                break;
            case InfoType.IronCount:
                thisText.text = string.Format("{0:F0}", GameManager.instance.IronCount);
                break;
            case InfoType.GoldCount:
                thisText.text = string.Format("{0:F0}", GameManager.instance.GoldCount);
                break;
            case InfoType.DiamondCount:
                thisText.text = string.Format("{0:F0}", GameManager.instance.DiamondCount);
                break;
            case InfoType.ChargeBar:
                Player p = GameManager.instance.player;
                float per = Mathf.Lerp(0f, 1f, p.SkillTimer / p.SkillCoolTime[p.id]);
                thisSlider.value = per;
                image.color = ChargeColor(per);
                break;
            case InfoType.UserGold:
                TextMeshProUGUI.text = GetThousandCommaText(UserInfoManager.Instance.userData.GameMoney);
                break;
            case InfoType.UserGem:
                TextMeshProUGUI.text = GetThousandCommaText(UserInfoManager.Instance.userData.GameGem);
                break;
            case InfoType.UserName:
                thisText.text = UserInfoManager.Instance.userData.UserName;
                break;
            case InfoType.DailyNotice:
                thisimage.enabled = (UserInfoManager.Instance.userData.TodayStamp) ? false : true;
                break;
            case InfoType.MissionNotice:
                thisimage.enabled = (MissionManager.instance.isReward) ? true : false;
                break;
            case InfoType.TodayProgress:
                TextMeshProUGUI.text = UserInfoManager.Instance.userData.TodayProgress.ToString();
                break;
            case InfoType.WeekProgress:
                TextMeshProUGUI.text = UserInfoManager.Instance.userData.WeekProgress.ToString();
                break;
            case InfoType.TodaySlider:
                thisSlider.value = UserInfoManager.Instance.userData.TodayMissionValue[index];
                break;
            case InfoType.WeekSlider:
                thisSlider.value = UserInfoManager.Instance.userData.WeekMissionValue[index];
                break;
            case InfoType.TodayRewardSlider:
                thisSlider.value = UserInfoManager.Instance.userData.TodayProgress;
                break;
            case InfoType.WeekRewardSlider:
                thisSlider.value = UserInfoManager.Instance.userData.WeekProgress;
                break;
            case InfoType.TodayMissionValue:
                TextMeshProUGUI.text = UserInfoManager.Instance.userData.TodayMissionValue[index].ToString() +"/"+ MissionManager.instance.TodaySlider[index].maxValue.ToString();
                break;
            case InfoType.WeekMissionValue:
                TextMeshProUGUI.text = UserInfoManager.Instance.userData.WeekMissionValue[index].ToString() + "/" + MissionManager.instance.WeekSlider[index].maxValue.ToString();
                break;
            case InfoType.TodayMissionImage:
                thisimage.color = (MissionManager.instance.TodaySlider[index].maxValue  <= UserInfoManager.Instance.userData.TodayMissionValue[index]) 
                    ? MissionManager.instance.ColorList[1] : MissionManager.instance.ColorList[0];
                    break;
            case InfoType.WeekMissionImage:
                thisimage.color = (MissionManager.instance.WeekSlider[index].maxValue <= UserInfoManager.Instance.userData.WeekMissionValue[index])
                    ? MissionManager.instance.ColorList[1] : MissionManager.instance.ColorList[0];
                break;
            case InfoType.TodayClearFocus:
                thisimage.enabled = (UserInfoManager.Instance.userData.TodayReward[index] == 1) ? true : false;
                break;
            case InfoType.WeekClearFocus:
                thisimage.enabled = (UserInfoManager.Instance.userData.WeekReward[index] == 1) ? true : false;
                break;

        }
    }

    Color color()
    {
        float per = Mathf.Lerp(0f,1f, GameManager.instance.curHp/ GameManager.instance.maxHp)/2;
        Color color = Color.HSVToRGB(per, 0.78f, 1f);
        return color;
    }

    Color ChargeColor(float per)
    {
        Color color = Color.HSVToRGB(per / 3, 0.71f, 1f);
        return color;
    }

    public string GetThousandCommaText(int data) 
    { 
        return string.Format("{0:#,0}", data); 
    }
}
