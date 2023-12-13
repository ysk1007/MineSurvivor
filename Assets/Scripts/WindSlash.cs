using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

public class WindSlash : MonoBehaviour
{
    public LayerMask whatisPlatform;
    public CircleCollider2D circleCollider2D;
    public float damage;
    public int per;
    public Vector3 dir;
    public Material material;
    [ColorUsage(true, true)]
    public Color NormalColor;
    [ColorUsage(true, true)]
    public Color BloodColor;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        material = GetComponent<SpriteRenderer>().material;
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        this.dir = dir;

        if (per > -1)
        {
            rigid.velocity = dir * 5f;
        }
    }

    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        //DestroryArea();
        Invoke("SelfOff", 2f);
    }

    void SelfOff()
    {
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (GameManager.instance.player.isSkill)
        {
            material.color = BloodColor;
        }
        else
        {
            material.color = NormalColor;
        }
    }

    void DestroryArea()
    {
        int radiusInt = Mathf.RoundToInt(circleCollider2D.radius);
        for (int i = -radiusInt; i <= radiusInt; i++)
        {
            for (int j = -radiusInt; j <= radiusInt; j++)
            {
                Vector3 CheckCellPos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                float distance = Vector2.Distance(transform.position, CheckCellPos) - 0.001f;

                if (distance <= radiusInt)
                {
                    Collider2D overCollider2d = Physics2D.OverlapCircle(CheckCellPos, circleCollider2D.radius, whatisPlatform);
                    if (overCollider2d != null)
                    {
                        overCollider2d.transform.GetComponent<Bricks>().MakeDot(CheckCellPos);
                    }
                }
            }
        }
    }
}
