using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Timer, Health, IronCount, GoldCount, DiamondCount, HealthText, ExpText, ChargeBar }
    public InfoType type;
    public Image image;

    Text thisText;
    Slider thisSlider;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        thisSlider = GetComponent<Slider>();
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
                thisText.text = string.Format("{0:F1}%", cExp / mExp * 100); // Format 0은 들어올 첫 번째 인자 값의 위치
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
                float Hp = GameManager.instance.curHp;
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
}
