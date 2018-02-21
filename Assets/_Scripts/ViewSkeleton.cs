using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewSkeleton : MonoBehaviour 
{

//	public Transform rootNode;
	public Transform[] childNodes;

	void OnDrawGizmos()
	{
		if (transform != null)
		{
			if(childNodes == null || childNodes.Length == 0)
			{
				//get all joints to draw
				PopulateChildren();
			}


			foreach (Transform child in childNodes)
			{

//				if (child == transform)
//				{
//					//list includes the root, if root then larger, green cube
//					Gizmos.color = Color.green * 2f;3
//					Gizmos.DrawCube(child.position, new Vector3(.06f, .06f, .06f));
//				}
//				else
//				{
//					Gizmos.color = Color.white * 4f;
//					Gizmos.DrawLine(child.position, child.parent.position);
//					Gizmos.color = Color.cyan * 2f;
//					Gizmos.DrawCube(child.position, new Vector3(.05f, .05f, .05f));
//					Gizmos.color = Color.red * 2f;
//					Gizmos.DrawRay (transform.position, transform.forward);
//				}
			}

		}
	}

	public void PopulateChildren()
	{
		childNodes = transform.GetComponentsInChildren<Transform>();
	}
}
