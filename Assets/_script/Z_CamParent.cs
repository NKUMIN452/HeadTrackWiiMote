using UnityEngine;
using System.Collections;

public class Z_CamParent : MonoBehaviour {

	public float mDamper = 1;
	public Vector2 nunchuckOffset;
	public Vector2 dpad;
	public GameObject notrackCamera;
	public GameObject trackCam;
	// Use this for initialization
	void Start () {
	
		nunchuckOffset = new Vector2 (Wii.GetNunchuckAnalogStick (1).x, Wii.GetNunchuckAnalogStick (1).y);


	}
	
	// Update is called once per frame
	void Update () {
	

		if (Wii.GetButton(1,"UP")){
			print ("button up");
			transform.Translate(0,0,.02f);
		}
		if (Wii.GetButton(1,"DOWN")){
			
			transform.Translate(0,0,-.02f);
		}
		if (Wii.GetButton(1,"LEFT")){
			
			transform.Translate(-.02f,0,0);
		}
		if (Wii.GetButton(1,"RIGHT")){
			
			transform.Translate(0.02f,0,0);
		}
		if (Wii.GetButtonDown(1, "A")){
			print ("Button A");
			if(notrackCamera.gameObject.activeSelf){
				print ("notrack cam active");
				notrackCamera.gameObject.SetActive(false);
			}else{
				print ("notrack cam no active");
				notrackCamera.gameObject.SetActive(true);
			}
		}
			transform.Translate (Wii.GetNunchuckAnalogStick(1).x/mDamper,0,Wii.GetNunchuckAnalogStick(1).y/mDamper);
	}
}
