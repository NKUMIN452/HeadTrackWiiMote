using UnityEngine;
using System.Collections;

public class Z_CamParent : MonoBehaviour {

	public float mDamper = 1;
	public Vector2 nunchuckOffset;
	// Use this for initialization
	void Start () {
	
		nunchuckOffset = new Vector2 (Wii.GetNunchuckAnalogStick (1).x, Wii.GetNunchuckAnalogStick (1).y);

	}
	
	// Update is called once per frame
	void Update () {
	
		print (Wii.GetNunchuckAnalogStick (1).y);
//		if (Wii.GetNunchuckButton(0,"C")){
//			transform.Translate(0,0,.001f);
//		}
//		if (Wii.GetNunchuckButton(0,"Z")){
//			transform.Translate(0,0,-.001f);
//		}

			transform.Translate (Wii.GetNunchuckAnalogStick(1).x/mDamper,0,Wii.GetNunchuckAnalogStick(1).y/mDamper);
	}
}
