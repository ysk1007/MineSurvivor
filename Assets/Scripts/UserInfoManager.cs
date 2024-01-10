using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserData //���� ������ Ŭ����
{
    public string UserName; //�г���
    public string UserId; //���� ���̵�
    public int GameMoney;
    public int GameGem;
    public int SelectCharacter;
}

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance;
    public bool DataExist = false;

    private string keyName = "UserData"; //Ű ��
    private string fileName = "UserData.ms"; //���� �̸�

    public UserData userData;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            // �ٸ� Instance�� �����ϸ� ���� gameObject�� �ı��Ѵ�.
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DataLoad();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DataSave() //������ ����
    {
        ES3.Save(keyName, userData);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //���� ��ο� ������ ���� ���
        {
            DataExist = true;
            ES3.LoadInto(keyName, userData); //�ε�
            UnlockManager.Instance.DataLoad();
        }
        else
        {
            DataSave(); //������ ����
            UnlockManager.Instance.DataCreate();
        }
    }
}
