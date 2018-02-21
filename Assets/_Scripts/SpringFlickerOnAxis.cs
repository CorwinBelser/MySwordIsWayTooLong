using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFlickerOnAxis : MonoBehaviour {
	private Vector3 original_position;
	public Vector3 axis;
	public float amplitude = 1f;
	private int sign = 1;
	public AnimationCurve a;
	public float TimeSeconds = 1f;
	private float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
		original_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = original_position + axis * sign * amplitude * a.Evaluate ((elapsedTime % TimeSeconds) / TimeSeconds);
		elapsedTime += Time.deltaTime;
		sign *= -1;
	}
}
