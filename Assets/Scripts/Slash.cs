using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float damage;
    public int per;
    public Vector3 dir;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        this.dir = dir;
    }
    void OnEnable() //��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��
    {
        Invoke("SelfOff", 0.4f);
    }

    void SelfOff()
    {
        Debug.Log("����");
        gameObject.SetActive(false);
    }


}
