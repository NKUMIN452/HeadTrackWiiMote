using UnityEngine;
using System.Collections;

public class Z_headHit : MonoBehaviour {


	public static Color hitColor = new Color(1,0,0,0);
	public Color startColor = new Color (.521f, .63f, .63f, .8f);


	// Use this for initialization
	void Start () {

		hitColor = new Color (.52f, .63f, .63f, .8f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public IEnumerator OnTriggerEnter(Collider thing){

		print ("hit head");
		hitColor = new Color (1, 0, .2f, .5f);
		yield return new WaitForSeconds (.1f);
		hitColor = new Color (.52f, .63f, .63f, .8f);

	}
}
