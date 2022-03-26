using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContent : MonoBehaviour
{  
    public GameObject Tiles;
    public GameObject Building;
    public int x;
    public int z;

    public void Reload() {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        var DbGrid = transform.parent.GetComponent<GridSystem>().dbConnection.ReadGrid(x,z);
        Debug.Log(DbGrid);
        Debug.Log(DbGrid.ToString());
        if(DbGrid == null){
            transform.parent.GetComponent<GridSystem>().dbConnection.SaveGrid(x,z,Random.Range(0,255),Random.Range(0,255));
            Reload();
        }
        else{
            Tiles = Instantiate(transform.parent.GetComponent<GridSystem>().temp);
            Tiles.transform.name = "Tiles";
            Tiles.transform.SetParent(gameObject.transform,false);
            Tiles.GetComponent<SpriteRenderer>().color = new Color(0,0,0);
            /*Building = Instantiate(transform.parent.GetComponent<GridSystem>().temp);
            Building.transform.name = "Building";
            Building.transform.SetParent(gameObject.transform,false);*/
        }
    }
    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    /*void OnBecameInvisible() {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
    void OnBecameVisible() { //lorque on aura enlever tout les objets d'une certaine distance, on fera ca (si il est pas vu bha on destroy ses enfants)
        Reload();
    }*/
}