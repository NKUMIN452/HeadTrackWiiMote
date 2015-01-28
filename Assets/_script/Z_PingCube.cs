using UnityEngine;
using System.Collections;

public class Z_PingCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		this.renderer.material.color = Z_headHit.hitColor;
	
	}

	public void OnMouseDown(){

		if(rigidbody){
			Vector3 knockPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1);
			this.rigidbody.AddExplosionForce (2000, knockPoint, 12);
		}

	}
}
