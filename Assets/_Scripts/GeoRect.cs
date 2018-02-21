using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoRect : MonoBehaviour {
	public int nX = 4;
	public int nY = 5;
	public float edgelength = .03f;

	private MeshFilter filter;

	public List<Vector3> verts;
	public List<int> tris;
	public List<Vector2> uvs;

	public bool externalUpdate = false;

	void Start() {
		verts = new List<Vector3> ();
		tris = new List<int> ();
	}

	void Update() {
		if (!externalUpdate) {
			//init
			nX = Mathf.Abs(nX);//without this you could crash unity (you can still crash if you scroll this too large)
			nY = Mathf.Abs(nY);

			verts = new List<Vector3>();
			tris = new List<int>();
			uvs = new List<Vector2>();
			
			filter = GetComponent<MeshFilter>();
			verts = GenerateVertices();//will calcuate our plane's verts and assign them here
			tris = GenerateTris();//Will calculate our Plane's Triangle groups <---
			uvs = GenerateUVs();//calculate our UVS
			
			filter.sharedMesh = GenerateMesh();
			filter.sharedMesh.RecalculateNormals();//new 
		}
	}

	public void SetVerts(Vector3[] _verts) {
		verts = new List<Vector3>(_verts);
		GenerateTris ();
		GenerateUVs ();
		GenerateNormals ();
		filter.sharedMesh = GenerateMesh ();
		filter.sharedMesh.RecalculateNormals ();
	}


	private List<Vector3> GenerateVertices()
	{
		List<Vector3> verts = new List<Vector3>();

		for (int y = 0; y < nY ; y++) //we will have n+1 vertices in a row if is n is the number of quads (y direction)
		{
			for (int x = 0; x < nX; x++) //we repeat this loop for the x Direction
			{
				verts.Add (new Vector3((float)-x*edgelength,(float)-y*edgelength,0)); //set to negative for the sake of visualization (preference)

			}

		}

		return verts;
	}

	private List<int> GenerateTris() {
		List<int> _tris = new List<int> ();
		int numVerts = nX * nY;
		for (int i = 0; i < numVerts; i++) {
			// if this is not the last vert in the row, or in the last row of verts
			if ((i + 1) % nX != 0 && (i+nX+1) < numVerts) {
				_tris.Add (i);  // first tri on this square
				_tris.Add (i + 1);
				_tris.Add (i + nX);


				_tris.Add (i + 1);
				_tris.Add (i + nX + 1);
				_tris.Add (i + nX);
			}
		}
		return _tris;
	}

	private List<Vector2> GenerateUVs() {
		Vector2[] uvs = new Vector2[verts.Count];
		for (int v = 0; v < nY; v++) {
			for (int u = 0; u < nX; u++) {
				uvs [u + v * nX] = new Vector2 ((float)u / (nX - 1), (float)v / (nX - 1));
			}
		}
		return new List<Vector2>(uvs);
	}

	private List<Vector3> GenerateNormals() {
		Debug.LogWarning ("Just a heads up, GeoRect doesn't actually compute normals yet, but instead just makes all normals transform.forward");
		Vector3[] normals = new Vector3[ verts.Count ];
		for( int n = 0; n < normals.Length; n++ )
			normals[n] = -Vector3.forward;

		return new List<Vector3> (normals);
	}

	private Mesh GenerateMesh()
	{

		Mesh mesh = new Mesh();

		mesh.SetVertices(verts);//new

		mesh.SetTriangles(tris,0);//new
		mesh.SetUVs(0,uvs);

		return mesh;
	}
}
