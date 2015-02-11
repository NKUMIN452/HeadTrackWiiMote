using UnityEngine;
using System.Collections;

public class Z_fingerTrack : MonoBehaviour {

	public Vector3 shooterOffset;
	public Vector3 saberOffset;
	public Transform fingerTrans;
	public float mult;
	float x;
	float y;
	public GameObject shooter;
	public Transform saber;

	// Use this for initialization
	void Start () {

		saberOffset = Wii.GetMotionPlus (1);
		Wii.CalibrateMotionPlus (1);

	}
	
	// Update is called once per frame
	void Update () {

		Wii.GetMotionPlus (1);
		Wii.CheckForMotionPlus(1);
		//shooterOffset = new Vector3 (this.transform.position.x - .35f, this.transform.position.y, this.transform.position.z);
		//fingerTrans.localPosition = new Vector3 (Wii.GetNunchuckAnalogStick (0).x, Wii.GetNunchuckAnalogStick (0).y, fingerTrans.localPosition.z);
		lSaber ();
	
	}

	public IEnumerator makeBall(GameObject shooter_obj){

		GameObject Lshtr = Instantiate(shooter_obj,shooterOffset,Quaternion.identity) as GameObject;
		Lshtr.transform.localEulerAngles = new Vector3(0,-12,0);
		Lshtr.rigidbody.AddRelativeForce(0,0,2000);
			
		yield return new WaitForSeconds (1.4f);
		Destroy (Lshtr);

	}

	public void lSaber(){

		saber.localEulerAngles = new Vector3 (Wii.GetMotionPlus (1).x * mult, Wii.GetMotionPlus(1).y * mult, Wii.GetMotionPlus(1).z * mult);
	}
}
