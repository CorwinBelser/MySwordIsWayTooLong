using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Geo_Grid : MonoBehaviour {

    private MeshFilter filter;
    [SerializeField]
    private int meshSize = 2;//Our plane will have n*n quads
    [SerializeField]
    private float meshScale = 1;//Each quad will be this size
   
    private List<Vector3> vertices; //this will contain all of our verts
    private List<int> triangles; //This will contain our tri groups
    private List<Vector2> uvs; //this will contain our UV data



    void Update ()
    {
        //init
        meshSize = Mathf.Abs(meshSize);//without this you could crash unity (you can still crash if you scroll this too large)
    
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        filter = GetComponent<MeshFilter>();
        vertices = GenerateVertices();//will calcuate our plane's verts and assign them here
        triangles = GenerateTriangles();//Will calculate our Plane's Triangle groups <---
        uvs = GenerateUVS();//calculate our UVS
        filter.sharedMesh = GenerateMesh();
        filter.sharedMesh.RecalculateNormals();//new 

    }

    private List<Vector3> GenerateVertices()
    {
        List<Vector3> verts = new List<Vector3>();
        
        for (int y = 0; y < meshSize +1 ; y++) //we will have n+1 vertices in a row if is n is the number of quads (y direction)
        {
            for (int x = 0; x < meshSize + 1; x++) //we repeat this loop for the x Direction
            {
                verts.Add (new Vector3(-x*meshScale,-y*meshScale,0)); //set to negative for the sake of visualization (preference)
               
            }
            
        }
       
        return verts;
    }

    private List<int> GenerateTriangles()
    {
        List<int> tris = new List<int>();
        int index = meshSize * meshSize; //N*N = I if N is the number of quads in a row
        for (int i = 0; i < index+ meshSize; i++)
        {
     
            if ((i + 1) % (meshSize + 1) != 0) {
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

    private List<Vector2> GenerateUVS()
    {
        List<Vector2> uvsTemp = new List<Vector2>();
        float count = (float)meshSize + 1f;
        for (int x = 0; x <count; x++)
        {
            for (int y = 0; y < count; y++)
            {
                uvsTemp.Add(new Vector2 (x*(1f/count),y*(1f/count)));
            }

        }
        return uvsTemp;
    }

    private Mesh GenerateMesh()
    {

        Mesh mesh = new Mesh();
       
        mesh.SetVertices(vertices);//new

        mesh.SetTriangles(triangles,0);//new
        mesh.SetUVs(0,uvs);

        return mesh;
    }

}
