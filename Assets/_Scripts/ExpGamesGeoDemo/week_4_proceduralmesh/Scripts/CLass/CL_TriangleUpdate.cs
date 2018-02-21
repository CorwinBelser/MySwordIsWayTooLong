using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class CL_TriangleUpdate : MonoBehaviour {

    private MeshFilter filter;
    [SerializeField]
    private List<Vector3> vertices;
    [SerializeField]
    public static int jeff;


    // Update is called once per frame
    void Start () {
        filter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>(new Vector3[3]);
        filter.mesh = GenerateMesh();	
	}

    void Update ()
    {
        for(int i = 0; i < vertices.Count; i++)
        {
            if (vertices[i]!= filter.mesh.vertices[i])
            {
                filter.mesh.SetVertices(vertices);
            }
        }
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        //mesh.SetVertices(vertices);
        mesh.SetTriangles(new List<int>()
        {
            0,1,2
        } , 0);
        return mesh;
    }
}
