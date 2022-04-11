using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private Transform Player;
    private int Distance;
    private GridSystem gridSystem;
    private void Start() {
       Player = GameObject.Find("Player").transform;
       gridSystem = Player.GetComponent<GridSystem>();
       Distance = gridSystem.chunkMaxDistance;
    }
    private void Update() {
        if (Vector3.Distance(new Vector3(Player.position.x,0,Player.position.z), transform.position) > Distance)
        {
            //Destroy(this.gameObject);
            //gridSystem.LoadChunks();
            //Debug.Log(gameObject);
        }
    }
}
