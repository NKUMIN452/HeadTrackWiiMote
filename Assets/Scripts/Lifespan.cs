using UnityEngine;
using System.Collections;

public class Lifespan : MonoBehaviour {
	int lifespan;
	// Use this for initialization
	void Start () {
	lifespan = 500;
	}
	
	// Update is called once per frame
	void Update () {
	lifespan--;
	if (lifespan <= 0)
		Destroy(gameObject);
	}
}
