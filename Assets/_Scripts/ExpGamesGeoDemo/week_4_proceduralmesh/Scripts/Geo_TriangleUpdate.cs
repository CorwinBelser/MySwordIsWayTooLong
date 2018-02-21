using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Geo_TriangleUpdate : MonoBehaviour {

    private MeshFilter filter;
    public List<Vector3> vertices;//new
    

    void Start () //new
    {
        filter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>(new Vector3[3]);//pass in a new array to set this
                                                    // list to have 3 0'd items
 
      
        filter.mesh = GenerateMesh();
        
    }
    void Update ()//new
    {
        //if our Vertices(vector3) list difers from the objects vertices...
        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertices[i] != filter.sharedMesh.vertices[i])
            {
                filter.sharedMesh.SetVertices(vertices);
                //Update vertices
            }
        }
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);//new

        mesh.SetTriangles(new List<int>()
        {
            0, 1, 2
        }, 0);

        return mesh;
    }

}
