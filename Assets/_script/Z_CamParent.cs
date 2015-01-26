using UnityEngine;
using System.Collections;

public class Z_CamParent : MonoBehaviour {

	public float mDamper = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

		transform.Translate (Input.GetAxis("Horizontal")/mDamper, 0, Input.GetAxis ("Vertical")/mDamper);

	}
}
