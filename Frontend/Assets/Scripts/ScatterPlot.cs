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
    [SerializeField] private Transform xAxis;
    [SerializeField] private Transform yAxis;
    [SerializeField] private Transform main;

    [SerializeField] private int xInc = 100;
    [SerializeField] private int yInc = 100;

    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject xValPrefab;
    [SerializeField] private GameObject yValPrefab;
    [SerializeField] private GameObject mongoObj;

    [SerializeField] private bool isBaseline = false;

    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI accuracy;
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
    int mostRecentBaselineResult;
    int mostRecentConcResult;

    private List<Point> points = new List<Point>();
    private List<string> ts = new List<string>();

    private List<List<PatientData>> results = new List<List<PatientData>>();
    private List<PatientData> baselineResults = new List<PatientData>();
    private List<PatientData> concussionResults = new List<PatientData>();

    // Start is called before the first frame update
    void Start()
    {
        //udpSocket = sceneManager.GetComponent<UdpSocket>();
        mongo = mongoObj.GetComponent<MongoDAO>();
        // Used to get rectangle which we can get the width and height of
        Rect rect = RectTransformUtility.PixelAdjustRect(main.GetComponent<RectTransform>(), canvas);
        // Get width and height of rectangle
        mainWidth = rect.width;
        mainHeight = rect.height;

        results = mongo.FindByUUID(Data.uuid);
        baselineResults = results[0];
        concussionResults = results[1];

        if(isBaseline)
        {
            mostRecentBaselineResult = baselineResults.Count - 1;
            strPoints = baselineResults[mostRecentBaselineResult].coords;
            date.text = "Date: "+ baselineResults[mostRecentBaselineResult].date;
            accuracy.text = baselineResults[mostRecentBaselineResult].accuracy + "%";
            GetPoints();
            DrawPoints();
        }
        else{
            if(concussionResults.Count > 0) {
                mostRecentConcResult = concussionResults.Count - 1;
                strPoints = concussionResults[mostRecentConcResult].coords;
                date.text = "Date: "+ concussionResults[mostRecentConcResult].date;
                accuracy.text = concussionResults[mostRecentConcResult].accuracy + "%";
                GetPoints();
                DrawPoints();
            }
        
        }
    }

    async void GetPoints()
    {
        maxX = 0;
        maxY = 0;
        minX = int.MaxValue;
        minY = int.MaxValue;
        
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

       xAmt = (maxX - minX) + 1;
       yAmt = (maxY - minY) + 1;

        xInc = (int) mainWidth / xAmt;
        yInc = (int) mainHeight / yAmt;

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
            obj.transform.SetParent(main);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(0.0f, 0.0f);
            rt.anchorMin = new Vector2(0.0f, 0.0f);
            rt.anchoredPosition3D = new Vector3(point.x, point.y, 0);
            point.pointObj = obj;
        }
    }

    void ClearPoints()
    {
        for(int i = main.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(main.transform.GetChild(i).gameObject);
        }
    }

    public void PreviousBaselineTest() {
        if(mostRecentBaselineResult > 0) {
            ChangeBaselineTest(-1);
        }
    }

    public void NextBaselineTest() {
        if(mostRecentBaselineResult < baselineResults.Count-1) {
            ChangeBaselineTest(1);
        }
    }

    public void PreviousConcussionTest() {
        if(mostRecentConcResult > 0) {
            Debug.Log("Prev");
            ChangeConcussionTest(-1);
        }
    }

    public void NextConcussionTest() {
        if(mostRecentConcResult < concussionResults.Count-1) {
            ChangeConcussionTest(1);
        }
    }

    void ChangeBaselineTest(int valueChange)
    {
        ClearPoints();
        points = new List<Point>();
        mostRecentBaselineResult += valueChange;
        strPoints = baselineResults[mostRecentBaselineResult].coords;
        date.text = "Date: "+ baselineResults[mostRecentBaselineResult].date;
        accuracy.text = baselineResults[mostRecentBaselineResult].accuracy + "%";
        GetPoints();
        DrawPoints();
    }

    void ChangeConcussionTest(int valueChange)
    {
        ClearPoints();
        points = new List<Point>();
        mostRecentConcResult += valueChange;
        strPoints = concussionResults[mostRecentConcResult].coords;
        date.text = "Date: "+ concussionResults[mostRecentConcResult].date;
        accuracy.text = concussionResults[mostRecentConcResult].accuracy + "%";
        GetPoints();
        DrawPoints();
    }

}
