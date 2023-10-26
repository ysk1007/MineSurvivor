using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // Camera.main.WorldToScreenPoint ½ºÅ©¸° ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥°èÀÇ ÁÂÇ¥·Î º¯°æ ÇØÁÜ
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
