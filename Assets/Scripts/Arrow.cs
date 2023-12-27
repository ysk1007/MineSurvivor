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

    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        trailRenderer.enabled = false;
        Invoke("TrailOn", 0.3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Bricks") || per == -1)
            return;
        per--;

        if (per == 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void TrailOn()
    {
        trailRenderer.enabled = true;
    }
}
