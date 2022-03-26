using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int Dimension;
    public GameObject Player;
    public Transform Map;
    private int[] corner;

    public GameObject temp;


    private void Start(){
        corner = new int[] {-Dimension, Dimension-1};
        int Px = Mathf.RoundToInt(Player.transform.position.x);
        int Pz = Mathf.RoundToInt(Player.transform.position.z);
        for (int x = -Dimension ; x < Dimension; x++) // -5 4
            for (int z = -Dimension; z < Dimension; z++)
                if (Map.transform.Find((x+Px)+","+(z+Pz)) == null)
                    new Grids(x+Px,z+Pz,Map,temp);
    }

    // Update is called once per frame
    void Update()
    {
        int Px = Mathf.RoundToInt(Player.transform.position.x);
        int Pz = Mathf.RoundToInt(Player.transform.position.z);
        foreach (int x in corner)
        {
            for (int z = -Dimension; z < Dimension; z++)
                if (Map.transform.Find((x+Px)+","+(z+Pz)) == null)
                    new Grids(x+Px,z+Pz,Map,temp);
            for (int z = -Dimension+1; z < Dimension-1; z++)
                if (Map.transform.Find((z+Px)+","+(x+Pz)) == null)
                    new Grids(z+Px,x+Pz,Map,temp);       
        }
    }
}
