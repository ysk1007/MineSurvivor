using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum InfoType { Iron, Gold, Diamond, Magnet, Boom }
    public InfoType type;

    private void Awake()
    {

    }

    void OnTriggerEnter2D(Collider2D collision) //피격 감지
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
        }
        gameObject.SetActive(false);
    }
}
