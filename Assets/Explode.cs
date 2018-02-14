using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
	public GameObject explosion;
	public GameObject spark;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.GetComponent<Rigidbody>()){
			other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000f,other.gameObject.transform.position, 10f, 1f);
			GameObject boom = Instantiate(explosion,transform.position,Quaternion.identity) as GameObject;
			Destroy(boom, 1);
		}
		else{
			GameObject boom = Instantiate(spark,transform.position,Quaternion.identity) as GameObject;
			Destroy(boom, 1);
		}
	}
}
