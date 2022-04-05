using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

public class MongoDAO : MonoBehaviour
{
    // Connection string to access the mongo db
    private MongoClient client = new MongoClient("mongodb+srv://concussion-detector:gmitproject2022@cluster.rure1.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;


    void Start()
    {
        database = client.GetDatabase("concussionDB");
        collection = database.GetCollection<BsonDocument>("colBaseline");    
    }
    


    public ObjectId Id { get; set; }
    public string uuid {get; set; }
    public string coords {get; set; }

    
    

    
}
