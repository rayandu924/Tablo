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
        var gridContent = new DbConnection("Map", "MapCollection").ReadGrid(x,z);
        if(gridContent == null)
            new DbConnection("Map", "MapCollection").SaveGrid(x,z,0,0);

        /*Tiles = Instantiate(TilesPrefab);
        Tiles.transform.name = "Tiles";
        Tiles.transform.SetParent(gameObject.transform,false);
        Building = Instantiate(BuildingPrefab);
        Building.transform.name = "Building";
        Building.transform.SetParent(gameObject.transform,false);*/
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
