using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SkinnedMeshRenderer))]
[RequireComponent (typeof(LineRenderer))]
public class JankyTrailRender : MonoBehaviour {
	public float lifetime = .2f; //lifetime of a point on the trail
	private float normalLength = 1f;
	public float minimumVertexDistance = 0.1f; //minimum distance moved before a new point is solidified.

	public Vector3 velocity; //direction the points are moving

	SkinnedMeshRenderer smr;
	Vector3[] verts;
	public int vertindextodraw = 0;
	LineRenderer line;
	//position data
	List<Vector3> points;
	Queue<float> spawnTimes = new Queue<float>(); //list of spawn times, to simulate lifetime. Back of the queue is vertex 1, front of the queue is the end of the trail.
	//Length of this queue is one less than the number of positions in the linerenderer, since the linerenderer always has a non-solidified vertex at the object's position.

	public GeoRect renderGrid;

	// Use this for initialization
	void Awake () {
		smr = GetComponent<SkinnedMeshRenderer> ();
		SkinnedMesh mesh = GetComponent<SkinnedMesh>();
		mesh.OnResultsReady += DrawVertices;
		line = GetComponent<LineRenderer>();
		line.useWorldSpace = true;
		points = new List<Vector3>() { transform.position }; //indices 1 - end are solidified points, index 0 is always transform.position
//		renderGrid.SetVerts(points.ToArray());
	}

	void AddPoint(Vector3 position) {
		points.Insert(1, position);
		spawnTimes.Enqueue(Time.time);
	}

	void RemovePoint() {
		spawnTimes.Dequeue();
		points.RemoveAt(points.Count - 1); //remove corresponding oldest point at the end
	}

	// Update is called once per frame
	void Update () {
		//cull based on lifetime
		while(spawnTimes.Count > 0 && spawnTimes.Peek() + lifetime < Time.time) {
			RemovePoint();
		}

		//move positions
		Vector3 diff = -velocity * Time.deltaTime;
		for(int i = 1; i < points.Count; i++) {
			points [i] -= transform.position;
		}

		//add new point
		if (points.Count < 2 || Vector3.Distance(transform.position, points[1]) > minimumVertexDistance) {
			//if we have no solidified points, or we've moved enough for a new point
			AddPoint(transform.position);
		}

		//update index 0;
//		points[0] = transform.position;

		//save result
		line.positionCount = points.Count;
		line.SetPositions(points.ToArray());

		//do the thing to update the GeoGrid
//		renderGrid.SetVerts(points.ToArray());
	}

//	void OnDrawGizmos()
//	{
//		if (verts != null) {
//			foreach (Vector3 vert in verts) {
////			Gizmos.color = Color.white * 4f;
////			Gizmos.DrawLine(child.position, child.parent.position);
//		 		Gizmos.color = Color.cyan * 2f;
//				Gizmos.DrawCube (transform.TransformPoint (vert), new Vector3 (.05f, .05f, .05f));
//			}
//		}
//	}
//
	void DrawVertices(SkinnedMesh mesh)
	{
		Color color = Color.green;
		verts = mesh.vertices;
//		AddPoint( verts [vertindextodraw]);
		if (mesh.useBakeMesh)
		{
			var m = transform.localToWorldMatrix;
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				Vector3 position =  mesh.bakedVertices[i];
				Vector3 normal = mesh.bakedNormals[i];
				Debug.DrawLine(position, position + (normal * normalLength), color);
			}
		}
		else
		{
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				Vector3 position = mesh.vertices[i];
				Vector3 normal = mesh.normals[i];
				Debug.DrawLine(position, position + (normal * normalLength), color);
			}
		}

	}

}