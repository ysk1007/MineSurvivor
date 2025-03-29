using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum InfoType { Iron, Gold, Diamond, Magnet, Boom, ExpGem }
    public InfoType type;

    public Transform target;
    public float speed = 3f;
    public bool magnet = false;

    private void Awake()
    {
        target = GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (!magnet)
            return;

        if (target != null)
        {
            // 대상 오브젝트를 향해 방향을 구함
            Vector3 direction = (target.position - transform.position).normalized;

            // 이동
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")))
            PickUp();
    }

    public void PickUp()
    {
        switch (type)
        {
            case InfoType.Iron:
                GameManager.instance.IronCount++;
                break;
            case InfoType.Gold:
                GameManager.instance.GoldCount++;
                break;
            case InfoType.Diamond:
                GameManager.instance.DiamondCount++;
                break;
            case InfoType.ExpGem:
                GameManager.instance.GetExp();
                break;
        }
        magnet = false;
        gameObject.SetActive(false);
    }
}
