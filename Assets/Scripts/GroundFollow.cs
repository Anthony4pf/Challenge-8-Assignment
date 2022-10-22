using UnityEngine;
using System.Collections;

public class GroundFollow : MonoBehaviour {
	private Transform playerTransform;

	void Start ()
	{
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () 
	{
		transform.position = Vector3.forward * playerTransform.position.z;
	}
}
