using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindIntersection : MonoBehaviour
{
    public GameObject plane;
    public GameObject line;
    public GameObject intersection;
    public Vector3[] planeCorners;
    public Material planeMaterial;
    private Mesh mesh;
    public Vector3[] lineEnds;
    public float lineWidth;
    private LineRenderer lr;
    float dt;
    void Start()
    {
        SetupPlane();
        SetupLine();
        dt = Time.deltaTime;
    }

    Ray Mouse => Camera.main.ScreenPointToRay(Input.mousePosition+new Vector3(0,0,1));
    void Update()
    {
        var m = Mouse;
        lr.SetPosition(0, m.GetPoint(0));
        lr.SetPosition(1, m.GetPoint(100));
        FindIntersectionLocation();
    }

    void SetupPlane()
    {
        plane.AddComponent<MeshFilter>();
        plane.AddComponent<MeshRenderer>();
        mesh = plane.GetComponent<MeshFilter>().mesh;
        plane.GetComponent<MeshRenderer>().material = planeMaterial;
        mesh.Clear();

        mesh.vertices = new Vector3[]
        {
            planeCorners[0],
            planeCorners[1],
            planeCorners[2],
            planeCorners[3],
        };

        mesh.colors = new Color[]
        {
            new(0.8f, 0.8f, 0.8f, 1.0f),
            new(0.8f, 0.8f, 0.8f, 1.0f),
            new(0.8f, 0.8f, 0.8f, 1.0f),
            new(0.8f, 0.8f, 0.8f, 1.0f),
        };
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    }

    void SetupLine()
    {
        lr = line.GetComponent<LineRenderer>();
        lr.widthMultiplier = lineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, lineEnds[0]);
        lr.SetPosition(1, lineEnds[1]);
    }

    void FindIntersectionLocation()
    {
        Vector3[] verticies = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = verticies[triangles[1]];
        Vector3 p1 = verticies[triangles[0]];
        Vector3 p2 = verticies[triangles[2]];
        Vector3 la = lr.GetPosition(0);
        Vector3 lb = lr.GetPosition(1);

        Matrix4x4 M = new(
            new(la.x - lb.x, la.y - lb.y, la.z - lb.z, 0),
            new(p1.x - p0.x, p1.y - p0.y, p1.z - p0.z, 0),
            new(p2.x - p0.x, p2.y - p0.y, p2.z - p0.z, 0),
            new(0, 0, 0, 1)
        );
        M = M.inverse;
        Vector3 vector = new(la.x - p0.x, la.y - p0.y, la.z - p0.z);
        Vector3 tuv = M.MultiplyPoint(vector);
        float t = tuv.x;
        float u = tuv.y;
        float v = tuv.z;
        bool onLine = (t <= 1 && t >= 0);
        bool onPlane = (u <= 1 && u >= 0) && (v <= 1 && v >= 0);
        dt += Time.deltaTime;
        if (onLine && onPlane)
        {
            intersection.transform.position = la + (lb - la) * t; ;
            intersection.SetActive(true);
            
            if (Input.GetMouseButton(0) && dt > 0.25f)
            {
                GameObject.Instantiate(intersection);
            }
        } else
        {
            intersection.SetActive(false);
        }
    }
}
