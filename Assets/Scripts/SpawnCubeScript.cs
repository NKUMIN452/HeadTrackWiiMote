using UnityEngine;
using System.Collections;

public class SpawnCubeScript : MonoBehaviour {
	
	private float spawn = 0f;
	public Transform cube;
	private float pr = 50f;
	private float rr = 90f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
	spawn--;
	if (spawn <= 0)
	{
		spawn = Random.Range(50,150);
		Vector3 newPos = new Vector3(transform.position.x + Random.Range(-pr,pr), transform.position.y + Random.Range(-pr,pr) , transform.position.z);
		Quaternion newRot = new Quaternion(transform.rotation.x + Random.Range(-rr,rr) , transform.rotation.y + Random.Range(-rr,rr), transform.rotation.z 
			+ Random.Range(-rr,rr), transform.rotation.w );
		
		Transform newCube = (Transform)Instantiate(cube, newPos, newRot);
		newCube.rigidbody.AddForce(new Vector3(0,0,-20000));
		
	
	}
	
	
	}
}
