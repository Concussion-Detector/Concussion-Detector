using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseAccess : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://concussion-detector:gmitproject2022@cluster.rure1.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    // Start is called before the first frame update
    void Start()
    {
        database = client.GetDatabase("concussionDB");
        colBaseline = database.GetCollection<BsonDocument>("colBaseline");
        colConcussionTests = database.GetCollection<BsonDocument>("colConcussionTests");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
