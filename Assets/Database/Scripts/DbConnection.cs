using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using TMPro;
public class DbConnection
{
	const string dbUrl = "mongodb+srv://123:123@cluster0.vgila.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
	private string dbName;
	private string dbCollection;
	MongoClient client;
	IMongoDatabase database;
	public IMongoCollection<BsonDocument> collection;

	// Use this for initialization
	public DbConnection(string dbName, string dbCollection)
	{
		this.dbName =  dbName;
		this.dbCollection = dbCollection;
		client = new MongoClient(dbUrl);
		database = client.GetDatabase(dbName);
		collection = database.GetCollection<BsonDocument>(dbCollection);
	}

	public async Task<GridsJson> ReadGrids(int x,int z){
		FilterDefinition<BsonDocument> xfilter = Builders<BsonDocument>.Filter.Eq("x", x);
		FilterDefinition<BsonDocument> zfilter = Builders<BsonDocument>.Filter.Eq("z", z);
		var GridDocument = await collection.Find(xfilter & zfilter).FirstOrDefaultAsync();
		if (GridDocument == null)
			return null;
		var result = BsonSerializer.Deserialize<GridsJson>(GridDocument);
		return result;
	}
	public async Task SaveGrids(int x,int z,string tiles){
		var document = new BsonDocument
		{
		{"x",x},
		{"z",z},
		{"tiles", tiles},
		};
		await collection.InsertOneAsync(document);
	}
}
public class GridsJson{
	 public BsonObjectId _id;
     public int x;
     public int z;
     public string tiles;
};