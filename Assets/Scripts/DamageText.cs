using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshPro Text;
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("SelfOff", 0.5f);
    }
    void SelfOff()
    {
        gameObject.SetActive(false);
    }

    public void value(float val)
    {
        Text.text = string.Format("{0:F0}", val);
    }
}
