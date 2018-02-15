using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Continuous rotate takes rotation per second in degrees for x, y, and z, and then rotates this transform accordingly. 
/// Can be set active/inactive, and will generate a random rotationPerSecond with a given maximum magnitude.
/// </summary>
public class ContinousRotate : MonoBehaviour {
	public Vector3 rotation;
	public bool active = true;
	public bool randomizeRotation = false;
	public float randomRotationMagnitude = 90f;

	private Vector3 initialPosition;
	private Vector3 initialRotation;

	// Use this for initialization
	void Start () {
		if (randomizeRotation) {
			rotation.x = (Random.value * 2f - 1f) * randomRotationMagnitude;
			rotation.y = (Random.value * 2f - 1f) * randomRotationMagnitude;
			rotation.z = (Random.value * 2f - 1f) * randomRotationMagnitude;
		}
	}

	// Update is called once per frame
	void Update () {
//		transform.position = initialPosition + Mathf.Sin (Time.realtimeSinceStartup * timeFactor) * position;
		if (active) {
			transform.rotation *= Quaternion.Euler (Time.deltaTime * rotation);
		}
	}
}
