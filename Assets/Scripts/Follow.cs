using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    public float x;
    public float y;
    public float z;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // Camera.main.WorldToScreenPoint ��ũ�� ��ǥ�� ���� ��ǥ���� ��ǥ�� ���� ����
        Vector3 vc = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        rect.position = new Vector3(vc.x + x, vc.y + y, vc.z + z);
    }
}
