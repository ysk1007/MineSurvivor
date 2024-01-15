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
    public bool CharacterAble; // 해금 여부

    [Header(" # Status Info")]
    public int CharacterLevel;
    public int[] Damage; // 피해량
    public int[] Hp; // 최대 체력
    public string ATS; // 공격 속도
    public string Speed; // 이동 속도
    public string Range; // 사거리

    [Header(" # Skill icon")]
    public Sprite[] icons;

}
