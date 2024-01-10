using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData //���� ĳ���� ������
{
    public string CharacterName; // �̸�
    public int CharacterID; // ���� ��ȣ
    public bool CharacterAble; // �ر� ����
    public int CharacterLevel;
    public int[] Damage; // ���ط�
    public int[] Hp; // �ִ� ü��
    public string ATS; // ���� �ӵ�
    public string Speed; // �̵� �ӵ�
    public string Range; // ��Ÿ�
}

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;
    public bool DataExist = false;

    private string keyName = "UnlockData"; //Ű ��
    private string fileName = "UserData.ms"; //���� �̸�

    public List<CharacterData> OriCha_info; // �������� ĳ���� ����
    public List<CharacterData> UserUnlockData;

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
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DataSave() //������ ����
    {
        for (int i = 0; i < OriCha_info.Count - 1; i++)
        {
            ES3.Save<List<CharacterData>>(keyName, UserUnlockData);
        }
    }

    public void DataCreate() //������ ����
    {
        for (int i = 0; i < OriCha_info.Count - 1; i++)
        {
            ES3.Save<List<CharacterData>>(keyName, OriCha_info);
        }
        UserUnlockData = OriCha_info;
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //���� ��ο� ������ ���� ���
        {
            DataExist = true;
            UserUnlockData = ES3.Load<List<CharacterData>>(keyName);
        }
        else
        {
            DataCreate(); //������ ����
        }
    }
}
