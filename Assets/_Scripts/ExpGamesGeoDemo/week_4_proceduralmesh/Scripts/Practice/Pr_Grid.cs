using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Pr_Grid : MonoBehaviour {

    private MeshFilter filter;
    [SerializeField]
    private int meshSize = 2;//Our plane will have n*n quads
    [SerializeField]
    private float meshScale = 1;//Each quad will be this size

    private List<Vector3> vertices; //this will contain all of our verts
    private List<int> triangles; //This will contain our tri groups
    private List<Vector2> uvs;



    // Update is called once per frame
    void Update () {
        meshSize = Mathf.Abs(meshSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        filter = GetComponent<MeshFilter>();
        vertices = GenerateVertices();
        triangles = GenerateTriangles();
        uvs = GenerateUVS();
        filter.mesh = GenerateMesh();
        filter.sharedMesh.RecalculateNormals();
     
	}

    private List<Vector2> GenerateUVS()
    {
        List<Vector2> uvsTemp = new List<Vector2>();
        float count = (float)meshSize;
        for (int x = 0; x < count + 1f; x++)
        {
            for (int y = 0; y < count + 1f; y++)
            {
                uvsTemp.Add(new Vector2(x * (1f / count), y * (1f / count)));
            }
        }
        return uvsTemp;
    }

    private List<int> GenerateTriangles()
    {
        List<int> tris = new List<int>();
        int index = meshSize * meshSize;
        for( int i = 0; i < index+meshSize; i++)
        {
            if ((i+1) % (meshSize+1) != 0)
            {
                tris.Add(i);
                tris.Add(i + meshSize + 2);
                tris.Add(i + meshSize + 1);

                tris.Add(i);
                tris.Add(i + 1);
                tris.Add(i + meshSize + 2);
            }
        }
        return tris;
    }

    private List<Vector3> GenerateVertices()
    {
        List<Vector3> verts = new List<Vector3>();
        for(int y =0; y < meshSize+1; y++)
        {
            for (int x = 0; x < meshSize + 1; x++)
            {
                verts.Add(new Vector3(-x * meshScale, -y * meshScale, Mathf.Sin((y+1)*(x+1)*meshScale*Time.deltaTime*200)));
            }
        }
        return verts;
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        
        mesh.SetTriangles(triangles,0);
        mesh.SetUVs(0, uvs);
        return mesh;
    }
}
