using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneParticle : MonoBehaviour
{
    void OnEnable() //��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ��
    {
        Invoke("SelfOff", 1.5f);
    }

    void SelfOff()
    {
        gameObject.SetActive(false);
    }
}
