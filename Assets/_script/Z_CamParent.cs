using UnityEngine;
using System.Collections;

public class Z_CamParent : MonoBehaviour {

	public float mDamper = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
//		if (Wii.GetNunchuckButton(0,"C")){
//			transform.Translate(0,0,.001f);
//		}
//		if (Wii.GetNunchuckButton(0,"Z")){
//			transform.Translate(0,0,-.001f);
//		}
		transform.Translate (Wii.GetNunchuckAnalogStick(0).x/mDamper,0,Wii.GetNunchuckAnalogStick(0).y/mDamper);
	}
}
