using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "Scriptable Object/ArtifactData")]
public class ArtifactData : ScriptableObject
{
    public enum ArtifactRate { NORMAL, RARE, UNIQUE, LEGENDARY }

    [Header(" # Main Info")]
    public ArtifactRate Rate;
    public int ArtifactId;
    public string ArtifactName;
    [TextArea]
    public string ArtifactDesc;
    public Sprite ArtifactIcon;

    [Header(" # Status Info")]
    public int ArtiLevel;
    public int ArtiExp;
    public float[] baseDamge;
    public int[] baseCount;

    [Header(" # Object")]
    public GameObject projectile;

}