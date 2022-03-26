using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int Dimension;
    public GameObject Player;
    private int[] corner;

    public GameObject temp;
    public DbConnection dbConnection = new DbConnection("Map", "MapCollection1");

    private void Start(){
        corner = new int[] {-Dimension, Dimension};
        int Px = Mathf.RoundToInt(Player.transform.position.x);
        int Pz = Mathf.RoundToInt(Player.transform.position.z);
        for (int x = -Dimension ; x < Dimension; x++) // -5 4
            for (int z = -Dimension; z < Dimension; z++)
                if (transform.Find((x+Px)+","+(z+Pz)) == null)
                    new Grids(x+Px,z+Pz,transform);
    }

    // Update is called once per frame
    void Update()
    {
        int Px = Mathf.RoundToInt(Player.transform.position.x);
        int Pz = Mathf.RoundToInt(Player.transform.position.z);
        foreach (int x in corner)
        {
            for (int z = -Dimension; z <= Dimension; z++)
                if (transform.Find((Px+x)+","+(Pz+z)) == null)
                    new Grids(Px+x,Pz+z,transform);
            for (int z = -Dimension+1; z < Dimension; z++)
                if (transform.Find((Px+z)+","+(Pz+x)) == null)
                    new Grids(Px+z,Pz+x,transform);       
            
        }
    }
}
