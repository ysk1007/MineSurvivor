using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class Dynamite : MonoBehaviour
{
    public float damage;
    public int per;
    public Vector3 dir;
    Rigidbody2D rigid;
    public GameObject Expolsion;
    public Transform pos;
    public float rotationSpeed = 360.0f; // 초당 회전 속도 (90도/초로 설정)

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 오브젝트를 z축 주위로 회전
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            Instantiate(Expolsion, pos.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        //Invoke("SelfOff", 7f);
    }

    void SelfOff()
    {
        Debug.Log("종료");
        gameObject.SetActive(false);
    }
}
