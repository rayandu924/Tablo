using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using TMPro;
public class DbConnection 
{
	const string dbUrl = "mongodb+srv://123:123@cluster0.vgila.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
	private string dbName;
	private string dbCollection;
	MongoClient client;
	IMongoDatabase database;
	IMongoCollection<BsonDocument> collection;

	// Use this for initialization
	public DbConnection(string dbName, string dbCollection)
	{
		this.dbName =  dbName;
		this.dbCollection = dbCollection;
		client = new MongoClient(dbUrl);
		database = client.GetDatabase(dbName);
		collection = database.GetCollection<BsonDocument>(dbCollection);
	}

	public BsonDocument ReadGrid(int x,int z){
		FilterDefinition<BsonDocument> xfilter = Builders<BsonDocument>.Filter.Eq("x", x);
		FilterDefinition<BsonDocument> zfilter = Builders<BsonDocument>.Filter.Eq("z", z);
		BsonDocument GridDocument = collection.Find(xfilter & zfilter).FirstOrDefault();
		return BsonDocument.Parse(GridDocument.ToJson());
	}
	public void SaveGrid(int x,int z,int tile,int building){
		var document = new BsonDocument
		{
		{"x",x},
		{"z",z},
		{"tile", tile},
		{"building", building}
		};
		collection.InsertOne(document);
	}
}
