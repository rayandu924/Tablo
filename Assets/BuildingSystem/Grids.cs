using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids
{
    public int x;
    public int z;
    public GameObject grid;

    public Grids(int x, int z, Transform Map, GameObject temp)
    {
        grid = new GameObject();
        grid.transform.name = x+","+z;
        grid.transform.position = new Vector3(this.x,0,this.z);
        grid.transform.SetParent(Map,false);
        //Get the content of the grid (tile and building from database)
        grid.AddComponent<SpriteRenderer>();
        GridContent content = grid.AddComponent<GridContent>();
        content.x = x;
        content.z = z;
        content.Reload();
    }
}
