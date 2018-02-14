using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Rigidbody _rb;
	public float turnRate;
	public float speed;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		_rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
			transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * turnRate * Time.deltaTime);
			transform.Rotate(Vector3.right * Input.GetAxis("Mouse Y") * turnRate * Time.deltaTime);
			transform.Translate(Vector3.right * Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);
			transform.Translate(Vector3.forward * Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);
	}
	
}
