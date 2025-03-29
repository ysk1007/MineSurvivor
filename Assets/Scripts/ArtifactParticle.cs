using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactParticle : MonoBehaviour
{
    public GameObject NormalParticle;
    public GameObject SpecialParticle;
    public Artifact artifact;

    void Update()
    {
        if (!artifact.data)
        {
            NormalParticle.SetActive(false);
            SpecialParticle.SetActive(false);
        }
        else
        {
            if (artifact.data.Rate.GetHashCode() > 1)
            {
                NormalParticle.SetActive(false);
                SpecialParticle.SetActive(true);
            }
            else
            {
                NormalParticle.SetActive(true);
                SpecialParticle.SetActive(false);
            }
        }
    }
}
