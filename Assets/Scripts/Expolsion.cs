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

    void OnEnable() //��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��
    {
        Invoke("SelfOff", 2f);
    }

    void SelfOff()
    {
        gameObject.SetActive(false);
    }
}
