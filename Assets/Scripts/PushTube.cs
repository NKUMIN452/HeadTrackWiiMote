﻿using UnityEngine;
using System.Collections;

public class PushTube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	rigidbody.AddForce(new Vector3(0,0,-20000));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	if(transform.position.z < -5000)
	{
		transform.position = new Vector3(0,0,0);
	}
	
	}
}
