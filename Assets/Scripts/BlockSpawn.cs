using UnityEngine;
using System.Collections;

public class BlockSpawn : MonoBehaviour {
	public Transform shape1;
	public Transform shape2;
	public Transform shape3;
	public Transform shape4;
	private Transform[] shapes;
	private int spawnTime = 0;
	// Use this for initialization
	void Start () {
	
		shapes = new Transform[4]{shape1,shape2,shape3, shape4};
	
	}
	
	// Update is called once per frame
	void Update () {
	
	spawnTime--;
	
	if (spawnTime <- 0)
	{
		spawnTime = 200;
		Vector3 newPos = new Vector3(transform.position.x + Random.Range(0,2),transform.position.y,transform.position.z+ Random.Range(-1,2));
	
		Quaternion newRot = new Quaternion(transform.rotation.x + (Random.Range(0,2)*180),transform.rotation.y,transform.rotation.z + (Random.Range(0,2)*180), transform.rotation.w  );
		Debug.Log(newRot);
		Debug.Log(newPos);
		//newPos = transform.position;
		//newRot = transform.rotation;
			
			
		Instantiate(shapes[Random.Range(0,shapes.Length)], newPos, newRot);
	}
	
	}
}
