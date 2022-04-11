using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class GridSystem : MonoBehaviour
{
    public int chunkMaxDistance;
    public GameObject Map;
    public DbConnection dbConnection;
    public List<GameObject> Chunks = new List<GameObject>();
    public int noiseScale;
    public int maxHeight;

    private void Start() {
        Map = new GameObject("Map");
        dbConnection = new DbConnection("Map", "MapCollection");
        dbConnection.collection.Database.DropCollection("MapCollection");
        dbConnection.collection.Database.CreateCollection("MapCollection");
        dbConnection = new DbConnection("Map", "MapCollection");
        LoadChunks();
    }

    public void LoadChunks(){
        int Px = Mathf.RoundToInt((transform.position.x)/10)*10;
        int Pz = Mathf.RoundToInt((transform.position.z)/10)*10;
        for (int x = Px-chunkMaxDistance ; x <= Px+chunkMaxDistance; x += 10)
                for (int z = Pz-chunkMaxDistance ; z <= Pz+chunkMaxDistance; z += 10)
                    if(Map.transform.Find((x)+","+(z)) == null && Vector3.Distance(transform.position, new Vector3(x,0,z)) < chunkMaxDistance)
                        LoadChunk(x,z);
    }

    private async void LoadChunk(int _x, int _z){
        GameObject chunk = new GameObject(_x+","+_z);
        chunk.transform.parent = Map.transform; 
        chunk.transform.position = new Vector3(_x,0,_z);
        Chunks.Add(chunk);
        var TaskgridsJson = dbConnection.ReadGrids(_x,_z);
        var gridsJson = await TaskgridsJson;
        if (gridsJson == null){
            await dbConnection.SaveGrids(_x,_z, CreateRandomChunk(_x,_z));
            TaskgridsJson = dbConnection.ReadGrids(_x,_z);
            gridsJson = await TaskgridsJson;
        }
        List<string> grids = gridsJson.tiles.Split(';').ToList();
            for (int x = _x ; x < _x+10; x++)
                for (int z = _z; z < _z+10; z++)
                    CreateGrid(chunk,x,z,grids[(x-_x)*10+(z-_z)]);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach(GameObject chunk in Chunks.ToList())
            if (Vector3.Distance(transform.position, chunk.transform.position) > chunkMaxDistance)
            {
                Chunks.Remove(chunk);
                Destroy(chunk);
                LoadChunks();
                //Debug.Log(gameObject);
            }
    }

    public string CreateRandomChunk(int _x, int _z){
        string grids= "";
        for (float x = _x ; x < _x+10; x++)
            for (float z = _z; z < _z+10; z++)
                grids += PerlinNoise(x,z,30,3).ToString()+","+PerlinNoise(x,z,noiseScale,maxHeight).ToString()+","+0+";";
        return grids;
    }

    private void CreateGrid(GameObject chunk, int x, int z, string _gridData)
    {
        List<int> gridData = _gridData.Split(',').Select(Int32.Parse).ToList();
        GameObject grid = Instantiate(Resources.Load<GameObject>("GridPrefabs/0"), new Vector3(x,gridData[1],z), Quaternion.Euler(new Vector3(0,0,0)),chunk.transform);
        grid.transform.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/"+gridData[0]);
    }

    private int PerlinNoise(float x, float y, int noiseScale, int ampli){
        return Mathf.RoundToInt(Mathf.PerlinNoise(x/noiseScale,y/noiseScale)*ampli);
    }
}
