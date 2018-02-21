using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a deprocated All in one triangle creation script. It has been replaced by
/// triangles.cs and AbstractMeshGenerator.cs working in unison
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class UDT_Triangles1 : MonoBehaviour {

    [SerializeField]            //at this time there is no need for other scripts to access our material 
    private Material material;  //through this script So we have set material to private, but create it as
                                //a serialized feild to allow it to be interacted with in the inspector
    [SerializeField]
    private Vector3[] vs = new Vector3[3];
    private List<Vector3> vertices;
    private List<int> triangles;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    void Update()   //we are using the Update here to take advantage of [ExicuteInEditMode]
    {               //If we wanted to generate at runtime we wuld use Awake()

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = material;

        //initialize
        vertices = new List<Vector3>();
        triangles = new List<int>();
        //generate mesh
        CreateMesh();

    }

    private void CreateMesh()
    {
        mesh = new Mesh(); //Mesh is a class, not a monobehavior O:

        SetVertices();
        SetTriangles();

        //Vertices Must be set first
        mesh.SetVertices(vertices); //this is super confusing and would have thrown me off had I not done anther Tut before this
                                    //SetVertices Here is a built in fucntion seperate from our SetVertices() method O.o 
        mesh.SetTriangles(triangles, 0); //same

        mesh.RecalculateNormals();  //running this method will automatically calculate normals for your mesh (and does a good job
                                    //if you are looking for smoothsurface) this allows the object to interact with scene lighting

        meshFilter.mesh = mesh; //Share Mesh changes all instances of this mesh while mesh would create an instance
    }

    private void SetVertices ()
    {
        vertices.AddRange(vs);
    }

    private void SetTriangles()
    {
        triangles.Add(0); //reverse order will flip forward face
        triangles.Add(1);
        triangles.Add(2);
    }


}
