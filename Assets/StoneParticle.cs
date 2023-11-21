using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneParticle : MonoBehaviour
{
    void OnEnable() //스크립트가 활성화 될 때 호출
    {
        Invoke("SelfOff", 1.5f);
    }

    void SelfOff()
    {
        gameObject.SetActive(false);
    }
}
