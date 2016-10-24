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
				if (graphicsHands [i].GetHand ().IsLeft && type.Equals(left)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
					hand_l = graphicsHands [i];
				}else if (graphicsHands [i].GetHand ().IsRight && type.Equals(right)) {
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
}
