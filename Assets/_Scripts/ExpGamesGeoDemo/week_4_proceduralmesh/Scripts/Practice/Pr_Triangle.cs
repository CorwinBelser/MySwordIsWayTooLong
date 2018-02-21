using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Pr_Triangle : MonoBehaviour {

    private MeshFilter filter;

	
	// Update is called once per frame
	void Update () {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();
	}

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(new List<Vector3>()
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(-1,0,0),
            new Vector3 (-1,1,0)
        });
        
        mesh.SetTriangles(new List<int>()
        {
            0,1,2,
            3,2,1
        },0);
        return mesh;
    }
}
