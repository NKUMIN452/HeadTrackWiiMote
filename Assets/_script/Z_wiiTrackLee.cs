using UnityEngine;
using System.Collections;

public class Z_wiiTrackLee : MonoBehaviour {

	bool mUseWiiMotes = true;                        // to disable wiiMote support

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
	public GameObject leftIR;
	public GameObject rightIR;
	public Transform bleft;
	public Transform topleft;
	public Transform bright;
	public Vector2 firstPoint = new Vector2();//first IR Point XY Position
	public Vector2 secondPoint = new Vector2();//second IR Point XY Position
	int numvisible = 0;

	//Z stuff
	public int pixel_width = 1680;
	public int pixel_height = 1050;
	private float screenAspect;
	//switches for perspective stuff
	public bool CAVE = false;
	public bool lookatswitch = true;
	public bool FOV = false;
	public bool Matrix = false;
	public Camera HeadCam;
	public float FOV1;
	public float FOVMult=1;
	public Transform lookAtObject;
	public float left = -0.2F;
	public float right = 0.2F;
	public float top = 0.2F;
	public float bottom = -0.2F;
	public float near = .05f;
	public float far = 100;
	public float gmult = 1.0f;

	// Use this for initialization
	void Start () {

		Wii.SetIRSensitivity (0, 90);
		screenAspect = pixel_width / pixel_height;
	}
	
	// Update is called once per frame
	void LateUpdate () {


		leftIR.transform.position = new Vector3 (firstPoint.x/1016, firstPoint.y/760, 0);
		rightIR.transform.position = new Vector3 (secondPoint.x/1016, secondPoint.y/760, 0);

		TrackHead ();
		if(Matrix){//only do this if the user chooses, for experimenting with getting the perspective correct


			Matrix4x4 m = PerspectiveOffCenter (
												near*(-.5f * screenAspect + mHeadX)* mHeadDist,//left
			                                    near*(.5f * screenAspect + mHeadX)* mHeadDist,//right
			                                    near*(-.5f - mHeadY) * mHeadDist,//bottom
			                                    near*(.5f - mHeadY) * mHeadDist,//top
			                                    near,//near
			                                    100//far
			                                    );

			HeadCam.projectionMatrix = m;
			HeadCam.projectionMatrix = HeadCam.projectionMatrix * Matrix4x4.Scale(new Vector3(1,1,1));
		}
		if(CAVE){

			Vector3 BottomLeftCorner = new Vector3(0 - mScreenWidthInMM/200, 0 - mScreenHeightInMM/100, -mHeadDist);
			Vector3 BottomRightCorner = new Vector3(0 + mScreenWidthInMM/200, 0 - mScreenHeightInMM/100, -mHeadDist);
			Vector3 TopLeftCorner = new Vector3(0 - mScreenWidthInMM/200, 0, -mHeadDist);
			Vector3 trackerPosition = mHeadPosition;


			Matrix4x4 genProjection = GeneralizedPerspectiveProjection(
				BottomLeftCorner,//0 - screenWidth/2, currentHeight - screen Height, mHeadDist
				BottomRightCorner,//0 + screenWidth/2, currentHeight - screen Height, mHeadDist
				TopLeftCorner,//0-screenWidth/2, 0, mHeadDist 
				trackerPosition,//mHeadX, mHeadY, -mHeadDist
				.05f,
				100
				);
			HeadCam.projectionMatrix = genProjection;  

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
			mHeadY = .5f + Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist;

		}else{
			mHeadY = -.5f + Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist;
		}

		//now apply that crap to the camera transforms
		//Vector3 newHeadPosition = new Vector3 (mHeadX, mHeadY, -mHeadDist);
		HeadCam.transform.localPosition = new Vector3 (mHeadX, mHeadY, -mHeadDist);
		//Vector3 lookAt = new Vector3(mHeadX , mHeadY , 0);//lookat point, currently being fakes      
		lookAtObject.localPosition = new Vector3 (0, mHeadY, 1);


		if (lookatswitch){//makes head cam look at lookat point when switched on
			HeadCam.transform.LookAt (lookAtObject);
		}


		//the following is Johnny Lee's Projection Matrix
		/*float nearPlane = .05f;
		device.Transform.Projection = Matrix.PerspectiveOffCenterLH(
			
			nearPlane*(-.5f * screenAspect + headX)/headDist,//left 
			nearPlane*(.5f * screenAspect + headX)/headDist,//right 
			nearPlane*(-.5f - headY)/headDist,//bottom 
			nearPlane*(.5f - headY)/headDist,//top
			nearPlane,//near
			100//far
			
			);*/

	}

	static Matrix4x4 PerspectiveOffCenter(
		float left, float right,
		float bottom, float top,
		float near, float far )
	{    
		//unity's projection matrix original
		float x =  (2.0f * near) / (right - left);
		float y =  (2.0f * near) / (top - bottom);
		float a =  (right + left) / (right - left);
		float b =  (top + bottom) / (top - bottom);
		float c = - (far + near) / (far - near);
		float d = -(2.0f * far * near )/ (far - near);
		float e = -1.0f;

		//D3DXMatrix values
//		float x =  (2.0f * near) / (right - left);
//		float y =  (2.0f * near) / (top - bottom);
//		float a =  (left + right) / (left - right);
//		float b =  (top + bottom) / (bottom-top);
//		float c =  (far) / (far - near);
//		float d =  (far * near) / (near - far);
//		float e =  1.0f;
		
		
		Matrix4x4 m = new Matrix4x4 ();

		//orig from Unity
//		m[0,0] = x;
//		m[0,1] = 0;
//		m[0,2] = a;
//		m[0,3] = 0;
//		m[1,0] = 0;
//		m[1,1] = y;
//		m[1,2] = b;
//		m[1,3] = 0;
//		m[2,0] = 0;
//		m[2,1] = 0;
//		m[2,2] = c;
//		m[2,3] = d;
//		m[3,0] = 0;
//		m[3,1] = 0;
//		m[3,2] = e;
//		m[3,3] = 0;

		m[0,0] = x;
		m[0,1] = 0;
		m[0,2] = a;
		m[0,3] = 0;
					m[1,0] = 0;
					m[1,1] = y;
					m[1,2] = b;
					m[1,3] = 0;
								m[2,0] = 0;
								m[2,1] = 0;
								m[2,2] = c;
								m[2,3] = d;
											m[3,0] = 0;
											m[3,1] = 0;
											m[3,2] = e;
											m[3,3] = 0;
										
		
		return m;
		
	}

	//CAVE guy stuff
	public static Matrix4x4 GeneralizedPerspectiveProjection(Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pe, float near, float far)

	{
		Vector3 va, vb, vc;
		Vector3 vr, vu, vn;
		
		float left, right, bottom, top, eyedistance;
		
		Matrix4x4 transformMatrix;
		Matrix4x4 projectionM;
		Matrix4x4 eyeTranslateM;
		Matrix4x4 finalProjection;
		
		///Calculate the orthonormal for the screen (the screen coordinate system
		vr = pb - pa;
		vr.Normalize();
		vu = pc - pa;
		vu.Normalize();
		vn = Vector3.Cross(vr, vu);
		vn.Normalize();
		
		//Calculate the vector from eye (pe) to screen corners (pa, pb, pc)
		va = pa-pe;
		vb = pb-pe;
		vc = pc-pe;
		
		//Get the distance;; from the eye to the screen plane
		eyedistance = -(Vector3.Dot(va, vn));
		
		//Get the varaibles for the off center projection
		left = (Vector3.Dot(vr, va)*near)/eyedistance;
		right  = (Vector3.Dot(vr, vb)*near)/eyedistance;
		bottom  = (Vector3.Dot(vu, va)*near)/eyedistance;
		top = (Vector3.Dot(vu, vc)*near)/eyedistance;
		
		//Get this projection
		projectionM = PerspectiveOffCenter(left, right, bottom, top, near, far);
		
		//Fill in the transform matrix
		transformMatrix = new Matrix4x4();
		transformMatrix[0, 0] = vr.x;
		transformMatrix[0, 1] = vr.y;
		transformMatrix[0, 2] = vr.z;
		transformMatrix[0, 3] = 0;
		transformMatrix[1, 0] = vu.x;
		transformMatrix[1, 1] = vu.y;
		transformMatrix[1, 2] = vu.z;
		transformMatrix[1, 3] = 0;
		transformMatrix[2, 0] = vn.x;
		transformMatrix[2, 1] = vn.y;
		transformMatrix[2, 2] = vn.z;
		transformMatrix[2, 3] = 0;
		transformMatrix[3, 0] = 0;
		transformMatrix[3, 1] = 0;
		transformMatrix[3, 2] = 0;
		transformMatrix[3, 3] = 1;
		
		//Now for the eye transform
		eyeTranslateM = new Matrix4x4();
		eyeTranslateM[0, 0] = 1;
		eyeTranslateM[0, 1] = 0;
		eyeTranslateM[0, 2] = 0;
		eyeTranslateM[0, 3] = -pe.x;
		eyeTranslateM[1, 0] = 0;
		eyeTranslateM[1, 1] = 1;
		eyeTranslateM[1, 2] = 0;
		eyeTranslateM[1, 3] = -pe.y;
		eyeTranslateM[2, 0] = 0;
		eyeTranslateM[2, 1] = 0;
		eyeTranslateM[2, 2] = 1;
		eyeTranslateM[2, 3] = -pe.z;
		eyeTranslateM[3, 0] = 0;
		eyeTranslateM[3, 1] = 0;
		eyeTranslateM[3, 2] = 0;
		eyeTranslateM[3, 3] = 1f;
		
		//Multiply all together
		finalProjection = new Matrix4x4();
		finalProjection = Matrix4x4.identity * projectionM*transformMatrix*eyeTranslateM;
		
		//finally return
		return finalProjection;
	}

	//test matrix, probably doesn't work
//	float scaleFov = (1+headPos.z/100)/(1+lastHeadPos.z/100);  //Calculate de zoom: dividing by lastHeadPos becouse the current zoom will be accumulated on de currentMatrix of the camera
//	Matrix4 headMatrix = Matrix4( 
//	                             scaleFov, 0, headDelta.x, headDelta.x*zNear,
//	                             0, scaleFov, headDelta.y, headDelta.y*zNear,
//	                             0, 0,  scaleFov,  0,
//	                             0, 0, 0, 1 );
}



