using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        GetData();    
    }

    public async Task<List<PatientData>> GetData()
    {
        var data = collection.FindAsync(new BsonDocument());
        var dataAwaited = await data;

        List<PatientData> allData = new List<PatientData>();

        foreach(var patientData in dataAwaited.ToList())
        {
            allData.Add(Deserialize(patientData.ToString()));
            //Debug.Log(Deserialize(patientData.ToString()));
            //Debug.Log(Deserialize(patientData.ToString()).uuid);

        }

        //Debug.Log(allData.ToString());

        return allData;
    }


    private PatientData Deserialize(string rawJson)
    {
        // Raw JSON
        // { "_id" : ObjectId("623a22b58145646f3baf3185"), "uuid" : "3054703f-08a5-4e87-ad8a-61a513182297", "coords" : "286, 218\n284, 217\n285, 217\n", "date" : "22/03/2022 19:25" }
        var dataToRetrieve = new PatientData();

        // Extracts the uuid from id
        // "uuid" : "3054703f-08a5-4e87-ad8a-61a513182297", "coords" : "286, 218\n284, 217\n285, 217\n", "date" : "22/03/2022 19:25" }
        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),")+3);
        Debug.Log(stringWithoutID);

        // 3054703f-08a5-4e87-ad8a-61a513182297
        var uuid = stringWithoutID.Substring(10,stringWithoutID.IndexOf("coords")-14);
        Debug.Log(uuid);

        var start = stringWithoutID.IndexOf("coords");
        var end = stringWithoutID.IndexOf("date");
        //stringWithoutID.ind
        Debug.Log("Get substring between " + start + " and " + end);
        //var coords = stringWithoutID.Substring(5,300);
        var coords = stringWithoutID.Substring(start,end);
        //var coords = stringWithoutID.Substring(stringWithoutID.IndexOf("coords")+11);
        //var coords = stringWithoutID.Substring(stringWithoutID.IndexOf("coords")+11,stringWithoutID.IndexOf("date")-16);
        //var coords = stringWithoutID.Substring(0,stringWithoutID.IndexOf("date")-16);
        //var coords = stringWithoutID.Substring(stringWithoutID.IndexOf("coords"),stringWithoutID.IndexOf("}")-stringWithoutID.IndexOf(":")-3);
        //Debug.Log(coords);

        dataToRetrieve.uuid = uuid;
        //dataToRetrieve.coords = coords;
        //dataToRetrieve.date = date;

        return dataToRetrieve;
    }
}

// Inline patient data class
public class PatientData
{
    public string uuid {get; set; }
    public string coords {get; set; }
    public string date {get; set; }

}
