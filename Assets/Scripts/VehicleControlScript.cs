using UnityEngine;
using System.Collections;

public class VehicleControlScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	private float speed = 30f;
	private float turn = 8f;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Input.GetAxis("Horizontal") > 0 )
		
		rigidbody.AddTorque(new Vector3(0,turn,0) );
		
		if (Input.GetAxis("Horizontal") < 0 )
			rigidbody.AddTorque(new Vector3(0,-turn,0));
		
		
		if (Input.GetAxis("Vertical") > 0)
			rigidbody.AddRelativeForce(new Vector3(0,-speed,0));
		
		if (Input.GetAxis("Vertical") < 0)
			rigidbody.AddRelativeForce(new Vector3(0,speed,0));
	
	}
}
