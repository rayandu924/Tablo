using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using TMPro;
public class DbConnection : MonoBehaviour {

	const string dbUrl = "mongodb+srv://123:123@cluster0.vgila.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
	public string dbName;
	public string dbCollection;

	MongoClient client = new MongoClient(dbUrl);
	IMongoDatabase database;
	IMongoCollection<BsonDocument> collection;
	// Use this for initialization
	void Start () {
		database = client.GetDatabase(dbName);
		collection = database.GetCollection<BsonDocument>(dbCollection);
		foreach (var item in client.ListDatabaseNames().ToList())
		{
			Debug.Log(item);
		}
	}
}
