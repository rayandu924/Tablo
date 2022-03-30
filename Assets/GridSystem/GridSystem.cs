using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int Dimension;
    public GameObject Player;
    public DbConnection dbConnection = new DbConnection("Map", "MapCollection");

    private void Start(){
        int Px = Mathf.RoundToInt(Player.transform.position.x);
        int Pz = Mathf.RoundToInt(Player.transform.position.z);
        for (int x = -Dimension ; x < Dimension; x++)
            for (int z = -Dimension; z < Dimension; z++)
                if (transform.Find((x+Px)+","+(z+Pz)) == null)
                    new Grids(x+Px,z+Pz,transform);
        Build();
    }

    // Update is called once per frame
    void Update()
    {
        int Px = Mathf.RoundToInt(Player.transform.position.x);
        int Pz = Mathf.RoundToInt(Player.transform.position.z);
        foreach (int x in new int[] {-Dimension, Dimension})
        {
            for (int z = -Dimension; z <= Dimension; z++)
                if (transform.Find((Px+x)+","+(Pz+z)) == null)
                    new Grids(Px+x,Pz+z,transform);
            for (int z = -Dimension+1; z < Dimension; z++)
                if (transform.Find((Px+z)+","+(Pz+x)) == null)
                    new Grids(Px+z,Pz+x,transform);       
        }
    }

    public void Build(){
        foreach (Transform child in transform)
        {
            
        }
    }
}