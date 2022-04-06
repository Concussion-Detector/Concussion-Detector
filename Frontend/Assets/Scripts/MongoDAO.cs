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

        //GetData();    
    }

    public async Task<List<PatientData>> GetData()
    {
        var data = collection.FindAsync(new BsonDocument());
        var dataAwaited = await data;

        List<PatientData> allData = new List<PatientData>();

        foreach(var patientData in dataAwaited.ToList())
        {
            allData.Add(Deserialize(patientData.ToString()));

        }

        Debug.Log(allData.ToString());

        return allData;
    }


    private PatientData Deserialize(string rawJson)
    {
        var dataToRetrieve = new PatientData();

        // Extracts the uuid from id
        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),")+4);
        var uuid = stringWithoutID.Substring(0,stringWithoutID.IndexOf(":")-2);
        var coords = stringWithoutID.Substring(stringWithoutID.IndexOf(":")+2,stringWithoutID.IndexOf("}")-stringWithoutID.IndexOf(":")-3);

        dataToRetrieve.uuid = uuid;
        dataToRetrieve.coords = coords;
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
