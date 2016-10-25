using UnityEngine;
using System.Collections;

public class ChangeSize : MonoBehaviour {

	public HandController hand_controller;
	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;
	private bool sumSize = false;
	private bool restSize = false;
	private bool rotRight = false;
	private bool rotLeft = false;
	private GameObject myCube; 

	// Use this for initialization
	void Start () {
		myCube = GameObject.Find ("Cube");
	}
	
	// Update is called once per frame
	void Update () {
		PlusSize ();
		RotateObject ();
	}

	float DistanceIndexs(){
		
		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length == 2) {
			
			Vector3 indexPositionLeft;
			Vector3 indexPositionRight;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft()) {
					indexPositionLeft = graphicsHands [i].fingers [1].GetTipPosition ();
				}
				if (graphicsHands [i].IsRight()) {
					indexPositionRight = graphicsHands [i].fingers [1].GetTipPosition ();
				}
			}

			float distance = (indexPositionLeft - indexPositionRight).magnitude;
			float normalizedDistance = (distance - 0.5f) / (1.7f - 0.5f);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			Debug.Log ("NormalizedDistance: " + normalizedDistance);
			return normalizedDistance;
		}

		return -1.0f;
	}

	float DistanceTipThumbIndex(string type){
		
		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

			Vector3 ThumbPosition;
			Vector3 IndexPosition;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && type.Equals("Left")) {
					ThumbPosition = graphicsHands [i].fingers [0].GetTipPosition();
					IndexPosition = graphicsHands [i].fingers [1].GetTipPosition();
				}else if (graphicsHands [i].IsRight() && type.Equals("Right")) {
					ThumbPosition = graphicsHands [i].fingers [0].GetTipPosition();
					IndexPosition = graphicsHands [i].fingers [1].GetTipPosition();
				}
			}

			float distance = (ThumbPosition - IndexPosition).magnitude;
			float normalizedDistance = (distance - 0.3f) / (0.7f - 0.3f);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			Debug.Log ("SizeDistance: " + normalizedDistance);
			return normalizedDistance;
		}
		return -1.0f;
	}

	float DistanceThumbIndex(string type){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

			Vector3 ThumbPosition;
			Vector3 IndexPosition;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && type.Equals("Left")) {
					ThumbPosition = graphicsHands [i].fingers [0].GetBoneCenter(3);
					IndexPosition = graphicsHands [i].fingers [1].GetJointPosition(1);
				}else if (graphicsHands [i].IsRight() && type.Equals("Right")) {
					ThumbPosition = graphicsHands [i].fingers [0].GetBoneCenter(3);
					IndexPosition = graphicsHands [i].fingers [1].GetJointPosition(1);
				}
			}

			float distance = (ThumbPosition - IndexPosition).magnitude;
			float normalizedDistance = (distance - 0.7f) / (0.9f - 0.7f);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			Debug.Log ("RotateDistance: " + normalizedDistance);
			return normalizedDistance;
		}
		return -1.0f;
	}

	void PlusSize(){
		
		float DistanceLeft = DistanceTipThumbIndex("Left");
		float DistanceRight = DistanceTipThumbIndex("Right");
		if (DistanceLeft == 0) {
			sumSize = true;
		}
		if (sumSize) {
			if (DistanceLeft == 1) {
				myCube.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
				myCube.transform.position += new Vector3 (0.0f,0.05f,0.0f);
				sumSize = false;
			}
		}
		if (DistanceRight == 0) {
			restSize = true;
		}
		if (restSize) {
			if (DistanceRight == 1) {
				myCube.transform.localScale += new Vector3 (-0.1f, -0.1f, -0.1f);
				myCube.transform.position += new Vector3 (0.0f,-0.05f,0.0f);
				restSize = false;
			}
		}
	}

	HandModel GetHand(string type){
		for (int i = 0; i < graphicsHands.Length; i++) {
			if (graphicsHands [i].IsLeft() && type.Equals("Left")) {
				return graphicsHands [i];
			}else if(graphicsHands [i].IsRight() && type.Equals("Right")){
				return graphicsHands [i];
			}
		}
		return null;
	}

	void RotateObject(){
		float DistanceLeft = DistanceThumbIndex("Left");
		Vector3 PositionLeft =  new Vector3(0f,0f,0f);
		HandModel handLeft = GetHand ("Left");
		if (DistanceLeft == 0 && handLeft != null && rotLeft==false) {
			Debug.Log ("Start Rotate");
			rotLeft = true;
			PositionLeft = handLeft.fingers [0].GetTipPosition (); 
			Debug.Log ("X: " + PositionLeft.x
			+ "Y: " + PositionLeft.y
			+ "Z: " + PositionLeft.z);
		}
		if (rotLeft) {
			Debug.Log ("Rotating");
			if (PositionLeft.x > handLeft.fingers [0].GetTipPosition ().x) {
				Debug .Log("Rotating Left");
				myCube.transform.Rotate(new Vector3 (0.0f, 5.0f, 0.0f));
			}
			if (DistanceLeft == 1) {
				rotLeft = false;
			}
		}

		float DistanceRight = DistanceThumbIndex("Right");
		Vector3 PositionRight =  new Vector3(0f,0f,0f);
		HandModel handRight = GetHand ("Right");
		if (DistanceRight == 0 && handRight != null && rotRight==false) {
			Debug.Log ("Start Rotate");
			rotRight = true;
			PositionRight = handRight.fingers [0].GetTipPosition (); 
			Debug.Log ("X: " + PositionRight.x
				+ "Y: " + PositionRight.y
				+ "Z: " + PositionRight.z);
		}
		if (rotRight) {
			Debug.Log ("Rotating");
			if (PositionRight.x < handRight.fingers [0].GetTipPosition ().x) {
				Debug .Log("Rotating Left");
				myCube.transform.Rotate(new Vector3 (0.0f, -5.0f, 0.0f));
			}
			if (DistanceRight == 1) {
				rotRight = false;
			}
		}
	}
}
