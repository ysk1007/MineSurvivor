using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float damage;
    public int per;

    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        Invoke("SelfOff", 0.5f);
    }

    void SelfOff()
    {
        Debug.Log("종료");
        gameObject.SetActive(false);
    }


}
