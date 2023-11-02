using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Expolsion : MonoBehaviour
{
    public float damage;
    public int per;
    public Vector3 dir;

    void Awake()
    {

    }

    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        Invoke("SelfOff", 2f);
    }

    void SelfOff()
    {
        gameObject.SetActive(false);
    }
}
