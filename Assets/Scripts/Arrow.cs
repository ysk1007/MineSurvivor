using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public int per;
    public Vector3 dir;
    Rigidbody2D rigid;
    public Transform pos;
    public TrailRenderer trailRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        this.dir = dir;

        if (per > -1)
        {
            rigid.velocity = dir * 7f;
        }
    }

    void OnEnable() //��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��
    {
        Invoke("SelfOff", 3f);
    }

    void SelfOff()
    {
        Debug.Log("����");
        gameObject.SetActive(false);
    }
}
