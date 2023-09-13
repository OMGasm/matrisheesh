using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRot : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;
    public Vector3 angle = new(0, 10, 0);
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        var verticies = mesh.vertices;
        angle.x++;
        angle.y+=0.6666f;
        angle.z+=0.1f;
        angle.x %= 10;
        angle.y %= 10;
        angle.z %= 10;
        Matrix4x4 R = Rotate(angle * Time.deltaTime);
        Matrix4x4 M = R;
        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i] = M.MultiplyPoint(verticies[i]);
        }
        mesh.vertices = verticies;
        mesh.RecalculateBounds();
    }

    Matrix4x4 Rotate(Vector3 angle)
    {
        var X = new Matrix4x4(
            new(1, 0, 0, 0),
            new(0, Mathf.Cos(angle.x), Mathf.Sin(angle.x), 0),
            new(0, -Mathf.Sin(angle.x), Mathf.Cos(angle.x), 0),
            new(0, 0, 0, 1));
        var Y = new Matrix4x4(
            new(Mathf.Cos(angle.y), 0, -Mathf.Sin(angle.y), 0),
            new(0, 1, 0, 0),
            new(Mathf.Sin(angle.y), 0, Mathf.Cos(angle.y), 0),
            new(0, 0, 0, 1));
        var Z = new Matrix4x4(
            new(Mathf.Cos(angle.z), Mathf.Sin(angle.z), 0, 0),
            new(-Mathf.Sin(angle.z), Mathf.Cos(angle.z), 0, 0),
            new(0, 0, 1, 0),
            new(0, 0, 0, 1));

        return X * Y * Z;
    }
}
