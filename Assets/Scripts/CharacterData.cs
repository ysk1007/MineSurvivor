using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/CharacterData")]
public class CharacterData : ScriptableObject
{ 
    [Header(" # Main Info")]
    public int CharacterID;
    public string CharacterName;

    [Header(" # Unlock Info")]
    public bool CharacterAble; // �ر� ����

    [Header(" # Status Info")]
    public int CharacterLevel;
    public int[] Damage; // ���ط�
    public int[] Hp; // �ִ� ü��
    public string ATS; // ���� �ӵ�
    public string Speed; // �̵� �ӵ�
    public string Range; // ��Ÿ�

    [Header(" # Skill icon")]
    public Sprite[] icons;

}
