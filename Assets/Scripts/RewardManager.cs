using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public bool newDay;
    public int attendance;
    public Animator[] RewardAnims; // ������ �ִϸ��̼�
    public GameObject[] TodayRewardFocus; // ������ ���� ��Ŀ��
    // Start is called before the first frame update
    void Start()
    {
        attendance = UserInfoManager.Instance.userData.Attendance;
        TodayRewardFocus[attendance].SetActive(true);
        int Today = 0;
        if (UserInfoManager.Instance.userData.TodayStamp)
        {
            Today++;
        }

        for (int i = 0; i < attendance + Today; i++)
        {
            RewardAnims[i].SetTrigger("Skip");
        }

        if (newDay)
        {
            this.gameObject.GetComponent<Popup>().PopupOne();
        }
    }

    public void Stamp(int index)
    {
        RewardAnims[index].SetTrigger("Stamp");
        UserInfoManager.Instance.Stamp();
    }

    public void Stamp()
    {
        RewardAnims[attendance].SetTrigger("Stamp");
        UserInfoManager.Instance.Stamp();
    }
}
