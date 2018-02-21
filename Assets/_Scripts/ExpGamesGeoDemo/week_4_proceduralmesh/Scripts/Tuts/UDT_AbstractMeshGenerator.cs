using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purpose of this script is to create an all purpose mesh creation class that can be inherrited by other classes.
/// It should contain everything that can be standardized across every 
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public abstract class UDT_AbstractMeshGenerator : MonoBehaviour {

    [SerializeField]            //at this time there is no need for other scripts to access our material 
    protected Material material;  //through this script So we have set material to private, but create it as
                                //a serialized feild to allow it to be interacted with in the inspector

    protected List<Vector3> vertices;
    protected List<int> triangles;

    protected List<Vector3> normals;
    protected List<Vector4> tangents;
    protected List<Vector2> uvs;
    protected List<Color32> vertexColours;

    protected int numVertices;
    protected int numTriangles;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Mesh mesh;

    void Update()   //we are using the Update here to take advantage of [ExicuteInEditMode]
    {               //If we wanted to generate at runtime we wuld use Awake()

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        meshRenderer.material = material;

        InitMesh();
        SetMeshNums();
        //generate mesh
        CreateMesh();

    }

    //the number of vertices and triangles will be different for each mesh, so this function must be overriden in you
    //mesh script that derrives from this class
    protected abstract void SetMeshNums();

    private bool ValidateMesh()
    {
        //build a string containign errors
        string errorStr = "";

        //Check that there are the correct number of triangles and vertices
        errorStr += vertices.Count == numVertices ? "" : "Should be" + numVertices + "vertices, but there are " + vertices.Count + ". ";
        //string add if triangles count == some pre determined number of triangles add nothing ELSE print this string. this is a "ternary operator" ¯\_(ツ)_/¯
        errorStr += triangles.Count == numTriangles ? "" : "Should be" + numTriangles + "vertices, but there are " + vertices.Count + ". ";
        //Num vertices.count should be equal to the number of normals (one normal per vert)
        errorStr += normals.Count == numVertices || normals.Count == 0 ? "" : "Should be" + numVertices + "normals, but there are " + normals.Count + ". ";
        errorStr += tangents.Count == numVertices || tangents.Count == 0 ? "" : "Should be" + numVertices + "normals, but there are " + tangents.Count + ". ";
        errorStr += uvs.Count == numVertices || uvs.Count == 0 ? "" : "Should be" + numVertices + "normals, but there are " + uvs.Count + ". ";
        errorStr += vertexColours.Count == numVertices || vertexColours.Count == 0 ? "" : "Should be" + numVertices + "normals, but there are " + vertexColours.Count + ". ";

        bool isValid = string.IsNullOrEmpty(errorStr);
        if (!isValid)
        {
            Debug.LogError("Not drawing mesh. " + errorStr);
        }
        return isValid;
    }

    private void InitMesh() {
        //initialize
        vertices = new List<Vector3>();
        triangles = new List<int>();
        //optional
        normals = new List<Vector3>();
        tangents = new List<Vector4>();
        uvs = new List<Vector2>();
        vertexColours = new List<Color32>();

    }


    private void CreateMesh()
    {
        mesh = new Mesh(); //Mesh is a class, not a monobehavior O:

        SetVertices();
        SetTriangles();
        SetNormals();
        SetTangets();
        SetUVs();
        SetVertexColors();

        if (ValidateMesh())
        {
            //Vertices Must be set first
            mesh.SetVertices(vertices); //this is super confusing and would have thrown me off had I not done anther Tut before this
                                        //SetVertices Here is a built in fucntion seperate from our SetVertices() method O.o 
            mesh.SetTriangles(triangles, 0); //same

            mesh.RecalculateNormals();  //running this method will automatically calculate normals for your mesh (and does a good job
                                        //if you are looking for smoothsurface) this allows the object to interact with scene lighting

            meshFilter.mesh = mesh; //Shared Mesh changes all instances of this mesh while mesh would create an instance
            meshCollider.sharedMesh = mesh;
        }
    }

    //these Methods must be overriden in the derrived class to perform actions specific to the Mesh being built
    protected abstract void SetVertices();
    protected abstract void SetTriangles();
    protected abstract void SetNormals();
    protected abstract void SetTangets();
    protected abstract void SetUVs();
    protected abstract void SetVertexColors();


}
