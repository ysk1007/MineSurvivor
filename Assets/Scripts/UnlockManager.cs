using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserCharacterData //���� ĳ���� ������
{
    public bool CharacterAble; // �ر� ����
    public int CharacterLevel;
}

[System.Serializable]
public class UserArtifactData //���� ĳ���� ������
{
    public bool ArtifactAble; // �ر� ����
    public bool ArtifactEquip; // ���� ����
    public int ArtifactLevel;
}

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;
    public bool DataExist = false;

    private string keyNameCha = "UnlockChaData"; //Ű ��
    private string keyNameArti = "UnlockArtiData"; //Ű ��
    private string fileName = "UserData.ms"; //���� �̸�

    public List<UserCharacterData> OriCha_info; // �������� ĳ���� ����
    public List<UserCharacterData> UserCharacterData;
    public List<UserArtifactData> OriArti_info; // �������� ��Ƽ��Ʈ ����
    public List<UserArtifactData> UserArtifactData;

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
            ES3.Save<List<UserCharacterData>>(keyNameCha, UserCharacterData);
        }
        for (int i = 0; i < 3; i++)
        {
            ES3.Save<List<UserArtifactData>>(keyNameArti, UserArtifactData);
        }
    }

    public void DataCreate() //������ ����
    {
        for (int i = 0; i < OriCha_info.Count - 1; i++)
        {
            ES3.Save<List<UserCharacterData>>(keyNameCha, OriCha_info);
        }
        for (int i = 0; i < 3; i++)
        {
            ES3.Save<List<UserArtifactData>>(keyNameArti, OriArti_info);
        }
        UserCharacterData = OriCha_info;
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName)) //���� ��ο� ������ ���� ���
        {
            DataExist = true;
            UserCharacterData = ES3.Load<List<UserCharacterData>>(keyNameCha);
            UserArtifactData = ES3.Load<List<UserArtifactData>>(keyNameArti);
        }
        else
        {
            DataCreate(); //������ ����
        }
    }
}
