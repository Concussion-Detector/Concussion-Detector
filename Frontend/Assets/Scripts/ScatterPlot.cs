using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScatterPlot : MonoBehaviour
{
    public class Point
    {
        public int x;
        public int y;
        public GameObject pointObj;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // Private variables
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform XAxis;
    [SerializeField] private Transform YAxis;
    [SerializeField] private Transform Main;

    [SerializeField] private int xInc = 100;
    [SerializeField] private int yInc = 100;

    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject xValPrefab;
    [SerializeField] private GameObject yValPrefab;
    private float mainWidth;
    private float mainHeight;

    private List<Point> points = new List<Point>();

    // Start is called before the first frame update
    void Start()
    {
        // Used to get rectangle which we can get the width and height of
        Rect rect = RectTransformUtility.PixelAdjustRect(Main.GetComponent<RectTransform>(), canvas);

        // Reduce the width and height by a number to add some padding
        mainWidth = rect.width - 100;
        mainHeight = rect.height - 100;

        // Create 50 random points to display
        CreateRandomPoints(50);
        // Draw those points on the graph
        DrawPoints();
        // Draw both axis
        DrawAxis((int)(mainWidth/xInc),(int)(mainHeight/yInc));
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

            xObj.GetComponentInChildren<Text>().text = (xSpacer * (i + 1)).ToString();
        }

        for(int i = 0; i <= yAxisCount; i++)
        {
            GameObject yObj = Instantiate(yValPrefab);

            yObj.transform.SetParent(YAxis);

            yObj.GetComponentInChildren<Text>().text = (ySpacer * (yAxisCount + 1 - i)).ToString();
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
