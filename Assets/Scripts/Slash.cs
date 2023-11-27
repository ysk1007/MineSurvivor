using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public LayerMask whatisPlatform;
    public CircleCollider2D circleCollider2D;
    public float damage;
    public int per;
    public Vector3 dir;
    public UnityEngine.Rendering.Universal.Light2D light2D;
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
    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        DestroryArea();
        StartCoroutine(SmoothDecreaseCoroutine());
        Invoke("SelfOff", 0.4f);
    }

    void SelfOff()
    {
        Debug.Log("종료");
        gameObject.SetActive(false);
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

    IEnumerator SmoothDecreaseCoroutine()
    {
        float timeElapsed = 0f;

        while (timeElapsed < 0.2f)
        {
            // Mathf.Lerp를 사용하여 1에서 0으로 자연스럽게 감소하는 값을 계산
            float Value = Mathf.Lerp(1f, 0f, timeElapsed / 0.2f);

            light2D.intensity = Value;

            // 시간 업데이트
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
