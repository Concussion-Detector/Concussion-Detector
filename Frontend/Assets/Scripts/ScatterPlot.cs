using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScatterPlot : MonoBehaviour
{
    // Private variables
    [SerializeField]private GameObject sceneManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform XAxis;
    [SerializeField] private Transform YAxis;
    [SerializeField] private Transform Main;

    [SerializeField] private int xInc = 100;
    [SerializeField] private int yInc = 100;

    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject xValPrefab;
    [SerializeField] private GameObject yValPrefab;
    [SerializeField] private GameObject mongoObj;

    [SerializeField] private bool isBaseline = false;

    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI name;
    private UdpSocket udpSocket;
    private MongoDAO mongo;
    private string[] strPoints;
    private int x, y;
    private int maxX = 0;
    private int maxY = 0;
    private int xAmt, yAmt;
    private int minX = int.MaxValue;
    private int minY = int.MaxValue;
    private float mainWidth;
    private float mainHeight;

    private List<Point> points = new List<Point>();
    private List<string> ts = new List<string>();

    private List<PatientData> results = new List<PatientData>();

    // Start is called before the first frame update
    void Start()
    {
        //udpSocket = sceneManager.GetComponent<UdpSocket>();
        mongo = mongoObj.GetComponent<MongoDAO>();
        // Used to get rectangle which we can get the width and height of
        Rect rect = RectTransformUtility.PixelAdjustRect(Main.GetComponent<RectTransform>(), canvas);
        // Get width and height of rectangle
        mainWidth = rect.width;
        mainHeight = rect.height;

        results = mongo.FindByUUID(Data.uuid);

        if(isBaseline)
        {
            strPoints = results[0].coords;
            Debug.Log("coords "+strPoints);
            date.text = "Date "+ results[0].date;
            //name.text = "ID: "+results[0].uuid;
            GetPoints();
            DrawPoints();
        }
        else{
            strPoints = results[1].coords;
            Debug.Log("coords "+strPoints);
            date.text = "Date "+ results[1].date;
            GetPoints();
            DrawPoints();

        }
    }

    async void GetPoints()
    {
       for(int i = 0; i < strPoints.Length - 1; i++)
       {
            var pts = strPoints[i].Split(',');
            x = int.Parse(pts[0]);
            y = int.Parse(pts[1]);
            if(x > maxX) {
                maxX = x;
            }

            if(y > maxY) {
                maxY = y;
            }

            if(x < minX) {
                minX = x;
            }

            if(y < minY) {
                minY = y;
            }
            if(x < 5) {
                Debug.Log(x);
            }
            points.Add(new Point(x, y));
       }

       Debug.Log("X ranges from " + minX + " to " + maxX);
       Debug.Log("Y ranges from " + minY + " to " + maxY);

       xAmt = (maxX - minX) + 1;
       yAmt = (maxY - minY) + 1;

       Debug.Log("We need points from 0 to " + xAmt + " on the x-axis");
       Debug.Log("We need points from 0 to " + yAmt + " on the y-axis");

        xInc = (int) mainWidth / xAmt;
        yInc = (int) mainHeight / yAmt;
        Debug.Log(mainWidth + " divided by " + xAmt + " = " + xInc);

       foreach (Point pt in points)
       {
           if(pt.x == minX) {
               pt.x = xInc;
           } else if(pt.x == maxX) {
               pt.x = xAmt * xInc;
           } else {
               pt.x = (pt.x - minX) * xInc;
           }

           if(pt.y == minY) {
               pt.y = yInc;
           } else if(pt.y == maxY) {
               pt.x = yAmt * yInc;
           } else {
               pt.y = (pt.y - minY) * yInc;
           }
       }
    }

    // Iterates through the list of points and instantiates
    // an object at that position which currently is a green 
    // circle
    void DrawPoints()
    {
        foreach (Point point in points)
        {
            GameObject obj = Instantiate(pointPrefab);
            obj.transform.SetParent(Main);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(0.0f, 0.0f);
            rt.anchorMin = new Vector2(0.0f, 0.0f);
            rt.anchoredPosition3D = new Vector3(point.x, point.y, 0);
            point.pointObj = obj;
        }
    }

    // Creates a given number of random points to display
    // This function won't be needed soon, but it is good for testing
    void CreateRandomPoints(int Count)
    {
        for(int i = 0; i < Count; i++)
        {
            points.Add(new Point(Random.Range(1, 500), Random.Range(1, 300)));
        }
    }
}
