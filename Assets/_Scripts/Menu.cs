using UnityEngine;
using System.Collections;
using Leap;

public class Menu : MonoBehaviour {
	
	public HandController hand_controller;
	public float min;
	public float max;

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;
	private HandModel hand_l;
	private HandModel hand_r;
	private GameObject cube_m;
	private GameObject sphere_m;
	private GameObject cylinder_m;
	private GameObject capsule_m;
	private float distance;
	private float distance1; //left thumb and right index
	private float distance2; //left index and right index
	private float distance3; //left middle and right index
	private bool isVisible = false;


	private string left = "LEFT";
	private string right = "RIGHT";
	private int thumb = 0;
	private int index = 1;
	private int middle = 2;
	private int ring = 3;
	private int pinky = 4;


	void Start () {
	
	}

	void Update () {
		displayMenu ();

	}

	void displayMenu(){
		distance = getFingerDistance (left, middle, ring, min, max);
		Debug.Log ("Menu Distance: " + distance);
		if (distance == 1) {
			showFingersIcons (true);
			isVisible = true;
		} else if (distance == 0 && isVisible) {
			showFingersIcons (false);
			isVisible = false;
		}
		//vane

		//distanceForObjects();
		distance1 = getFingerDistance1 (left, right, thumb, index, min, max);
		if (distance1 == 0) {
			Debug.Log ("Left thumb and right index touched!! with distance: " + distance1);
			createFigures ("cube_m");		
			//Debug.Log("The distance is different than 0, it is:  " + distance1);	
		} else {
			//Debug.Log ("Left thumb and right index touched!! with distance: " + distance1);
			//createFigures ("cube_m");		
			Debug.Log("The distance is different than 0, it is:  " + distance1);	
		}
	}

	void showFingersIcons(bool show){
		cube_m = GameObject.Find ("cube_m");
		cube_m.GetComponent<Renderer> ().enabled = show;
		sphere_m = GameObject.Find ("sphere_m");
		sphere_m.GetComponent<Renderer> ().enabled = show;
		cylinder_m = GameObject.Find ("cylinder_m");
		cylinder_m.GetComponent<Renderer> ().enabled = show;
	}
		
	float getFingerDistance(string type,int finger_1,int finger_2,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

			Vector3 firstFinger;
			Vector3 secondFinger;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && type.Equals(left)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
					hand_l = graphicsHands [i];
				}else if (graphicsHands [i].IsRight() && type.Equals(right)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
					hand_r = graphicsHands [i];
				}
			}

			float distance = (firstFinger - secondFinger).magnitude;
			float normalizedDistance = (distance - min) / (max - min);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			return normalizedDistance;

		}
		return -5.0f;
	}

	float getFingerDistance1(string type1, string type2, int finger_1,int finger_2,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 2) {

			Vector3 firstFinger;
			Vector3 secondFinger;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if ((graphicsHands [i].IsLeft() && type1.Equals(left)) && (graphicsHands [i + 1].IsRight() && type1.Equals(right)) 
					/*|| (graphicsHands [i].IsRight() && type1.Equals(right)) && (graphicsHands [i + 1].IsLeft() && type1.Equals(left))*/
				) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i + 1].fingers [finger_2].GetTipPosition ();
					hand_l = graphicsHands [i];
				}/*else if (graphicsHands [i].IsRight() && type.Equals(right)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
					hand_r = graphicsHands [i];
				}*/
			}
			float distance = (firstFinger - secondFinger).magnitude;
			float normalizedDistance = (distance - min) / (max - min);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			return normalizedDistance;

		}
		return -5.0f;
	}

	/*void verifyDistance (){
		double distance = getFingerDistance1(left, right, thumb, index, min, max);
		if (distance ==1) {
			Debug.Log ("Left thumb and right index touched!! with distance: " + distance);
		} else {
			Debug.Log("distance is more than 1!!!!! " + distance);
		}
	}*/

	/*string distanceForObjects(){
		distance1 = getFingerDistance1 (left, right, thumb, index, min, max);
		if (distance1 == 0) {
			Debug.Log ("Left thumb and right index touched!! with distance: " + distance);
			//createFigures ("cube_m");
			return "cube_m";
		} else {
			Debug.Log("The distance is more than 1!!!!! " + distance);
			return "-1";
		}

		distance2 = getFingerDistance1 (left, right, index, index, min, max);
		if (distance2 == 0) {
			Debug.Log ("Left index and right index touched!! with distance: " + distance2);
			createFigures ("sphere_m");
			return "sphere_m";
		} else {
			Debug.Log("The distance is more than 1!!!!! " + distance2);
			return "-1";
		}

		distance3 = getFingerDistance1 (left, right, middle, index, min, max);
		if (distance3 == 0) {
			Debug.Log ("Left middle and right index touched!! with distance: " + distance3);
			createFigures ("cylinder_m");
			return "cylinder_m";
		} else {
			Debug.Log("The distance is more than 1!!!!! " + distance3);
			return "-1";
		}
			
	return "hola";
	}*/

	void createFigures(string figure){
		//figure = distanceForObjects ();
		switch(figure){
		case "cube_m": 
			Debug.Log ("Cube selected");
			GameObject cube_m = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube_m.gameObject.GetComponent<Renderer> ().material.color = Color.blue;
			cube_m.transform.position = new Vector3 (0, 0, 0);
	
			break;
		case "sphere_m": 
			Debug.Log ("Sphere selected");
			break;
		case "cylinder_m":
			Debug.Log ("Cylinder selected");
			break;
		default:
			Debug.Log ("Nothing selected");
			break;
		}

	}
}