using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public GameObject popup;
    public TextMeshProUGUI inputTxt;

    void Start()
    {
        if (UserInfoManager.Instance.DataExist)
        {
            Invoke("EnterGame",1f);
        }
        else
        {
            popup.SetActive(true);
        }
    }

    void EnterGame()
    {
        SceneManager.LoadScene("lobby_Scene");
    }

    public void Naming()
    {
        UserInfoManager.Instance.userData.UserName = inputTxt.text;
        UserInfoManager.Instance.DataSave();
        popup.SetActive(false);
        Invoke("EnterGame", 1f);
    }
}
