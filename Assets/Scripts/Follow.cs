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
        // Camera.main.WorldToScreenPoint ½ºÅ©¸° ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥°èÀÇ ÁÂÇ¥·Î º¯°æ ÇØÁÜ
        Vector3 vc = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        rect.position = new Vector3(vc.x + x, vc.y + y, vc.z + z);
    }
}
