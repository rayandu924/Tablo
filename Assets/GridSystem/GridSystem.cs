using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class GridSystem : MonoBehaviour
{
    public int Dimension;
    public GameObject Player;
    public GameObject Map;
    public List<GameObject> Chunks;
    public int MaxChunks;


    public DbConnection dbConnection = new DbConnection("Map", "MapCollection");

    private void Start() {
        MaxChunks = (Dimension*2+1)*(Dimension*2+1);
        LoadChunks();
    }

    private void LoadChunks(){
        int Px = ((int)(((int)(Player.transform.position.x)/10))*10);
        int Pz = ((int)(((int)(Player.transform.position.z)/10))*10);
        for (int x = Px-(10*Dimension) ; x <= Px+(10*Dimension); x += 10) //10*dimension apres
                for (int z = Pz-(10*Dimension) ; z <= Pz+(10*Dimension); z += 10)
                    if(transform.Find((x)+","+(z)) == null){
                        LoadChunk(x,z);
                    }
    }

    private async void LoadChunk(int _x, int _z){
        GameObject chunk = new GameObject(); 
        Chunks.Add(chunk);
        chunk.transform.name = _x+","+_z;
        chunk.transform.SetParent(Map.transform,true);
        chunk.transform.position = new Vector3(_x,0,_z);
        var TaskgridsJson = dbConnection.ReadGrids(_x,_z);
        var gridsJson = await TaskgridsJson;
        if (gridsJson == null){
            await dbConnection.SaveGrids(_x,_z, CreateDataGrids());
            TaskgridsJson = dbConnection.ReadGrids(_x,_z);
            gridsJson = await TaskgridsJson;
        }
        List<int> grids = gridsJson.tiles.Split(',').Select(Int32.Parse).ToList();
                for (int x = _x ; x < _x+10; x++)
                    for (int z = _z; z < _z+10; z++)
                        CreateGrid(chunk,x,z,grids[(x-_x)*10+(z-_z)]);
        if(Chunks.Count > MaxChunks){
            GameObject obj = FarthestChunk(Chunks);
            Chunks.Remove(obj);
            Destroy(obj.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        LoadChunks();
    }

    public string CreateDataGrids(){
        string grids = (UnityEngine.Random.Range(0,3)).ToString();
                for (int i = 0; i < 99; i++)
                    grids += ","+(UnityEngine.Random.Range(0,3)).ToString();
        return grids;
    }

    private void CreateGrid(GameObject chunk, int x, int z, int gridData)
    {
        GameObject grid = Instantiate(Resources.Load<GameObject>("GridPrefabs/"+gridData));
        grid.transform.name = x+","+z;
        grid.transform.position = new Vector3(x,0,z);
        grid.transform.SetParent(chunk.transform,true);
        //need to reload when players make changes
    }

    
    private GameObject FarthestChunk(List<GameObject> Objects)
    {
        GameObject result = null;
        float maxDist = 0;
        foreach (GameObject obj in Objects)
        {   
            float dist = Vector3.Distance(Player.transform.position, obj.transform.position);
            if (dist > maxDist)
            {
                maxDist = dist;
                result = obj;
            }
        }
        Debug.Log(result.name);
        return result;
    }
}
