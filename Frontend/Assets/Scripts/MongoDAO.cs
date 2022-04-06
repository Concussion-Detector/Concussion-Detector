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
            //Debug.Log(Deserialize(patientData.ToString()).uuid);
            //Debug.Log(Deserialize(patientData.ToString()).date);

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
        //Debug.Log(stringWithoutID);

        // 3054703f-08a5-4e87-ad8a-61a513182297
        var uuid = stringWithoutID.Substring(10,stringWithoutID.IndexOf("coords")-14);
        Debug.Log(uuid);

        // 286, 218\n284, 217\n285, 217
        var coords = stringWithoutID.Substring(stringWithoutID.IndexOf("coords")+11,stringWithoutID.IndexOf("date")-67);
        // Split coords by the letter n (unable to split by \n)
        var points = coords.Split('n');
    
        // Iterate through each point and remove the \
        // The last point doesn't have a \ so we skip it
        for(int i = 0; i < points.Length; i++) {
            if(i != points.Length - 1) {
                points[i] = points[i].Substring(0,points[i].Length-1);
            }
            Debug.Log(points[i]);
        }

        // 22/03/2022 19:25
        var date = stringWithoutID.Substring(stringWithoutID.IndexOf("date")+9);
        date = date.Substring(0, date.Length-3);
        //Debug.Log(date);

        dataToRetrieve.uuid = uuid;
        dataToRetrieve.coords = coords;
        dataToRetrieve.date = date;

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
