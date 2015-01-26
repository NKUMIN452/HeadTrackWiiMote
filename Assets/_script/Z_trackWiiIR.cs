using UnityEngine;
using System.Collections;

public class Z_trackWiiIR : MonoBehaviour {

	public Transform point1;
	public Transform point2;
	public float distx;
	public float disty;
	public float distz;
	public Camera headcam;
	public Vector3 [] rawirdata = new Vector3[4];
	public Vector3 irdataleft;
	public Vector3 irdataright;
	public float z_dist_init;
	public float z_dist_update;
	public float z_dist_converted;
	public float mult;
	public Vector3 campos;


	// Use this for initialization
	void Start () {

		if(Wii.HasMotionPlus(0)){
			print ("Has Motion Plus");
			Wii.DeactivateMotionPlus (0);
		}
		z_dist_init = Vector3.Distance (point1.position, point2.position);
		mult = 4.0f;
	
	}
	
	// Update is called once per frame
	void Update () {

		rawirdata  = Wii.GetRawIRData (0);
		distx = rawirdata [0].x - rawirdata [1].x;
		disty = rawirdata [0].y - rawirdata [1].y;
		distz = Mathf.Sqrt ((distx * distx) + (disty * disty));

		irdataleft = Wii.GetIRVector3 (0);
		z_dist_update = Vector3.Distance (point1.position, point2.position);


		point1.position = new Vector3 (Wii.GetIRPosition(0).x*mult, Wii.GetIRPosition(0).y*-mult, distz*(mult*20));
		campos = point1.position;
		headcam.transform.localPosition = point1.position;

	}
}
