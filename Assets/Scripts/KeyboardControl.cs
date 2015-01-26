using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour {

	private int speed = 30;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Input.GetAxis("Horizontal") > 0)
			rigidbody.AddForce(new Vector3(speed,0,0));
			
		if (Input.GetAxis("Horizontal") < 0)
			rigidbody.AddForce(new Vector3(-speed,0,0));
			
		if (Input.GetAxis("Vertical") < 0)
			rigidbody.AddForce(new Vector3(0,0,-speed));
			
		if (Input.GetAxis("Vertical") > 0)
			rigidbody.AddForce(new Vector3(0,0,speed));
	
	}
	
	void OnCollisionExit()
	{
		
		rigidbody.AddForce(new Vector3(0,300,0));
	}
	
	void OnTriggerEnter(Collider c)
	{
		
		if (c.gameObject.CompareTag("Finish"))
		{
			transform.position = new Vector3(transform.position.x,65, transform.position.z);
		}
	}
	
}
