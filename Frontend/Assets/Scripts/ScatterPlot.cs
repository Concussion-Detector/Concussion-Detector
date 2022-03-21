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
    }
}
