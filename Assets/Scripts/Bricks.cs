using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bricks : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void MakeDot (Vector3 Pos)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(Pos);
        tilemap.SetTile(cellPosition, null);
    }

/*    void OnTriggerEnter2D(Collider2D collision) //피격 감지
    {
        if ((!collision.CompareTag("Slash") && !collision.CompareTag("Expolsion")))
            return;
        Collider2D overCollider2d = Physics2D.OverlapBox(collision.transform.position,collision.transform.localScale, 1);
        if (overCollider2d != null)
        {
            this.GetComponent<Bricks>().MakeDot(collision.transform.position);
        }
    }*/
}
