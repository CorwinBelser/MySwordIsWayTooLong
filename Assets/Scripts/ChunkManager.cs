using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    public List<Chunk> chunks;
    public List<SnapPoint> openPoints;
    public Vector3 pointPos;

    // Use this for initialization
    void Start() {
        openPoints = new List<SnapPoint>();

        CollectOpenPoints();
    }

    // Update is called once per frame
    void Update() {
        foreach (Chunk chunk in chunks)
        {
            foreach(SnapPoint point in chunk.snapPoints)
            {
                SnapPoint p = openPoints[Random.Range(0, openPoints.Count - 1)];
               // pointPos = new Vector3(p.chunk.transform.position)
                p.chunk.transform.position  = point.transform.position;
                p.open = false;
               // p. = false;
            }
            CollectOpenPoints();
        }

       /* for (int i = 0; i < chunks.Count; i++)
        {
            chunks[i] // this is chunk from the previous foreach loop
        } */

    }

    private void CollectOpenPoints()
    {
        foreach (Chunk chunk in chunks)
        {
            foreach (SnapPoint point in chunk.snapPoints)
            {
                if (point.open)
                    openPoints.Add(point);
                if (!point.open)
                {
                    openPoints.Remove(point);
                }
            }
        }
    }
    
}
