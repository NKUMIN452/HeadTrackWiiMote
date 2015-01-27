using UnityEngine;
using System.Collections;

public class Z_wiiTrackLee : MonoBehaviour {

	bool mUseWiiMotes = true;                        // to disable wiiMote support
	public bool lookatswitch = true;
	public bool FOV = false;
	public Vector3 mHeadPosition = new Vector3(0,0,0);           // position of the users head when the last frame was rendered
	public float mHeadX = 0;                            // last calculated X position of the users head
	public float mHeadY = 0;                            // last calculated Y position of the users head
	public float mHeadDist = 0;                            // last calculated Z position of the users head
	const float mRadiansPerPixel = (Mathf.PI / 4.0f) / 1024.0f;    // don't change this! it's a fixed value for the WiiMote infrared camera
	public float mIRDotDistanceInMM = 240.5f;                // distance of the IR dots in mm. change it, if you are not using the original nintendo sensor bar//*this is set to the Lee shop glasses
	public float mScreenHeightInMM = 353.0f;
	public float mScreenWidthInMM = 353.0f;                // height of your screen
	// height of your screen
	public bool mWiiMoteIsAboveScreen = true;                    // is the WiiMote mounted above or below the screen?
	public float mWiiMoteVerticleAngle = 0;                    // vertical angle of your WiiMote (as radian) pointed straight forward for me.

	//vectors from the wii IR lights
	public GameObject left;
	public GameObject right;
	public Vector2 firstPoint = new Vector2();//first IR Point XY Position
	public Vector2 secondPoint = new Vector2();//second IR Point XY Position
	int numvisible = 0;

	public Camera HeadCam;
	public float FOV1;
	public float FOVMult=1;
	public Transform lookAtObject;

	// Use this for initialization
	void Start () {

		Wii.SetIRSensitivity (0, 90);
	}
	
	// Update is called once per frame
	void LateUpdate () {


		left.transform.position = new Vector3 (firstPoint.x/1016, firstPoint.y/760, 0);
		right.transform.position = new Vector3 (secondPoint.x/1016, secondPoint.y/760, 0);

		TrackHead ();
		if (!Wii.IsActive(0)){
			print ("Lost Connection To WiiMote!!!");
		}

	
	}


	public void TrackHead(){



		//load points based on Raw IR Data Vector 2 Values
		firstPoint.x = (Wii.GetRawIRData (0) [0].x) * 1016;
		firstPoint.y = (Wii.GetRawIRData (0) [0].y) * 760;
		secondPoint.x = (Wii.GetRawIRData (0) [1].x) * 1016;
		secondPoint.y = (Wii.GetRawIRData (0) [1].y) * 760;


		float dx = firstPoint.x - secondPoint.x;
		float dy = firstPoint.y - secondPoint.y;
		float pointDist = Mathf.Sqrt (dx * dx + dy * dy);
		float angle = mRadiansPerPixel * pointDist / 2;

		//stuff I really don't understand that calculates where your head is
		mHeadDist = ((mIRDotDistanceInMM/2)/Mathf.Tan(angle))/mScreenHeightInMM;

		float avgX = (firstPoint.x + secondPoint.x)/2.0f;
		float avgY = (firstPoint.y + secondPoint.y)/2.0f;

		mHeadX = (Mathf.Sin(mRadiansPerPixel * (avgX - 512)) * mHeadDist);

		float relativeVerticalAngle = (avgY - 384) * mRadiansPerPixel;

		if(mWiiMoteIsAboveScreen){
			mHeadY = .5f + Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) *mHeadDist;

		}else{
			mHeadY = -.5f + Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist;
		}

		//now apply that crap to the camera transforms
		Vector3 newHeadPosition = new Vector3 (mHeadX, mHeadY, -mHeadDist);
		HeadCam.transform.localPosition = newHeadPosition;
		Vector3 lookAt = new Vector3(mHeadX/1.1f , mHeadY/1.1f , 0);//lookat point, currently being fakes
		lookAtObject.localPosition = new Vector3 (mHeadX / 1.1f, mHeadY / 1.1f, 0);


		if (lookatswitch){//makes head cam look at lookat point when switched on
			HeadCam.transform.LookAt (lookAtObject);
		}

		//the following is Johnny Lee's Projection Matrix
		/*float nearPlane = .05f;
		device.Transform.Projection = Matrix.PerspectiveOffCenterLH(
			
			nearPlane*(-.5f * screenAspect + headX)/headDist,//m0 
			nearPlane*(.5f * screenAspect + headX)/headDist,//m1 
			nearPlane*(-.5f - headY)/headDist,//m2 
			nearPlane*(.5f - headY)/headDist,//m3 
			nearPlane, 100//m4
			
			);*/

		//the following is from the Ogre 3D guy
		if (FOV){//turns on FOV Projection Matrix stuff

			FOV1 = (107 - .1944f * mHeadDist * mScreenHeightInMM/10) * FOVMult;

			float designedScreenWidth = mScreenWidthInMM; // I built my UI based on a 640px wide screen
			float distanceToUI = mHeadDist; // My UI elements are 500 units from the perspective camera
			float hFOVrad = Mathf.Atan((designedScreenWidth/2f)/distanceToUI);
			float aspect = Screen.width/Screen.height; // capture the CURRENT camera aspect ratio
			float vFOVdeg = Mathf.Atan(Mathf.Tan(hFOVrad/2f)/aspect) * 2f * Mathf.Rad2Deg;
			HeadCam.fieldOfView = FOV1;
		}

	}

}

//var startPosition : Vector3;
//var currentPosition : Vector3;
//
//
//function Start () {
//	startPosition = Camera.main.transform.position;
//	var r : Rect = Camera.main.pixelRect;
//	Debug.Log(r);
//}
//
//
//function LateUpdate () {
//	
//	var cam : Camera = camera;
//	cam.farClipPlane = 20;
//	cam.nearClipPlane = 0.5;
//	//camera.transform.LookAt(GameObject.Find("room").transform.position, Vector3.up);
//	//  camera.transform.LookAt(Vector3(cam.transform.position.x, cam.transform.position.y, 0), Vector3.down);
//	
//	var left : float =  cam.nearClipPlane * (-0.5 * 16 + cam.transform.position.x) / cam.transform.position.z;
//	var right : float = cam.nearClipPlane * (0.5 * 16 + cam.transform.position.x) / cam.transform.position.z;
//	var bottom : float = cam.nearClipPlane * (-0.5 * 9 + cam.transform.position.y) / cam.transform.position.z;
//	var top : float = cam.nearClipPlane * (0.5 * 9 + cam.transform.position.y) / cam.transform.position.z;
//	
//	
//	var m : Matrix4x4 = PerspectiveOffCenter(
//		left, right, bottom, top,
//		cam.nearClipPlane, 20 );  
//	
//	cam.projectionMatrix = m;
//	Debug.Log(m);
//	
//}
//
//static function PerspectiveOffCenter(
//	left : float, right : float,
//	bottom : float, top : float,
//	near : float, far : float ) : Matrix4x4
//{    
//	//unity's projection matrix
//	var x : float =  (2.0 * near) / (right - left);
//	var y : float =  (2.0 * near) / (top - bottom);
//	var a : float =  (right + left) / (right - left);
//	var b : float =  (top + bottom) / (top - bottom);
//	var c : float = -(far + near) / (far - near);
//	var d : float = -(2.0 * far * near) / (far - near);
//	var e : float = -1.0;
//	
//	var m : Matrix4x4;
//	m[0,0] = x;  m[0,1] = 0;  m[0,2] = a;  m[0,3] = 0;
//	m[1,0] = 0;  m[1,1] = y;  m[1,2] = b;  m[1,3] = 0;
//	m[2,0] = 0;  m[2,1] = 0;  m[2,2] = c;  m[2,3] = d;
//	m[3,0] = 0;  m[3,1] = 0;  m[3,2] = e;  m[3,3] = 0;
//	
//	return m;
//	
//}
