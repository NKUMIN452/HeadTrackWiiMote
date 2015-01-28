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

		saberOffset = new Vector3 (Wii.GetNunchuckAcceleration (0).x, Wii.GetNunchuckAcceleration (0).y, Wii.GetNunchuckAcceleration (0).z);

	}
	
	// Update is called once per frame
	void Update () {

		shooterOffset = new Vector3 (this.transform.position.x - .35f, this.transform.position.y, this.transform.position.z);
		fingerTrans.localPosition = new Vector3 (Wii.GetNunchuckAnalogStick (0).x, Wii.GetNunchuckAnalogStick (0).y, fingerTrans.localPosition.z);
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

		saber.localEulerAngles = new Vector3 ((Wii.GetNunchuckAcceleration (0).y - saberOffset.y) *mult, (Wii.GetNunchuckAcceleration (0).z - saberOffset.z) *mult,  (Wii.GetNunchuckAcceleration (0).x - saberOffset.x) *-mult);
	}
}
