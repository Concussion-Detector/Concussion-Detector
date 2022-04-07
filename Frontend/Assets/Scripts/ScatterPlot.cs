using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScatterPlot : MonoBehaviour
{
    // public class Point
    // {
    //     public int x;
    //     public int y;
    //     public GameObject pointObj;

    //     public Point(int x, int y)
    //     {
    //         this.x = x;
    //         this.y = y;
    //     }
    // }

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
    private UdpSocket udpSocket;
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

    // Start is called before the first frame update
    void Start()
    {
        //udpSocket = sceneManager.GetComponent<UdpSocket>();
        // Used to get rectangle which we can get the width and height of
        Rect rect = RectTransformUtility.PixelAdjustRect(Main.GetComponent<RectTransform>(), canvas);

        // Reduce the width and height by a number to add some padding
        mainWidth = rect.width - 50;
        mainHeight = rect.height - 20;
    }

    /*void Update()
    {   
        if(strPoints != null) {
            return;
        }
        else {
            if(udpSocket.GetPoints() != null)
            {
                strPoints = udpSocket.GetPoints();
                GetPoints();
                DrawPoints();
                Debug.Log("Main Width is " + mainWidth + " and Main Height is "  + mainHeight);
                DrawAxis(xAmt-1, yAmt-1);
            }
        }
    }*/

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

       foreach (Point pt in points)
       {
           if(pt.x == minX)
           {
               pt.x = xInc;
           } else if(pt.x == maxX)
           {
               pt.x = xAmt * xInc;
           } else {
               pt.x = (pt.x - minX) * xInc;
           }

           if(pt.y == minY)
           {
               pt.y = yInc;
           } else if(pt.y == maxY)
           {
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

    // Iterates through both the x-axis and the y-axis
    // instantiating an object with text with an incremental
    // number to describe the points
    void DrawAxis(int xAxisCount, int yAxisCount)
    {
        int xSpacer = xInc;
        int ySpacer = yInc;

        for(int i = 0; i <= xAxisCount; i++)
        {
            GameObject xObj = Instantiate(xValPrefab);

            xObj.transform.SetParent(XAxis);

            //xObj.GetComponentInChildren<Text>().text = (xSpacer * (i + 1)).ToString();
            xObj.GetComponentInChildren<Text>().text = (i + 1).ToString();
        }

        for(int i = 0; i <= yAxisCount; i++)
        {
            GameObject yObj = Instantiate(yValPrefab);

            yObj.transform.SetParent(YAxis);

            //yObj.GetComponentInChildren<Text>().text = (ySpacer * (yAxisCount + 1 - i)).ToString();
            yObj.GetComponentInChildren<Text>().text = (yAxisCount + 1 - i).ToString();
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
